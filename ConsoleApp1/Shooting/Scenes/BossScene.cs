using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BossScene : Scene, IBulletCreator
{
    private Player _player;
    private Map _map;
    private List<Boss> _bosses = new List<Boss>();
    private List<Bullet> _bullets = new List<Bullet>();
    private List<Monster> _monsters = new List<Monster>();

    private bool _isGameOver;
    private bool _isVictory;
    private int _phase;

    // 분열 연출
    private bool _isSplitting;
    private float _splitTimer;
    private const float k_SplitDuration = 2.0f;
    private Position _splitOrigin;

    // 페이즈3 전환 연출
    private bool _isPhase3Intro;
    private float _phase3IntroTimer;
    private const float k_Phase3IntroDuration = 2.0f;

    private int _totalMonstersToKill;
    private int _monstersKilled;

    private float _shakeTimer;
    private Random _random = new Random();

    public event GameAction BossDefeated;
    public event GameAction PlayAgainRequested;

    public BossScene(Player player)
    {
        _player = player;
    }

    public override void Load()
    {
        _phase = 1;
        _isGameOver = false;
        _isVictory = false;
        _isSplitting = false;
        _shakeTimer = 0;

        _map = new Map(this);
        AddGameObject(_map);

        _player.SetScene(this);
        _player.SetPosition(new Position(30, 22));
        AddGameObject(_player);

        SpawnPhase1Boss();
    }

    public override void Unload()
    {
        ClearGameObjects();
        _bosses.Clear();
        _bullets.Clear();
        _monsters.Clear();
    }

    public override void Update(float deltaTime)
    {
        if (_isGameOver)
        {
            if (Input.IsKeyDown(ConsoleKey.Enter))
            {
                _player.Reset();
                PlayAgainRequested?.Invoke();
            }
            return;
        }

        if (_isVictory)
        {
            if (Input.IsKeyDown(ConsoleKey.Enter))
            {
                BossDefeated?.Invoke();
            }
            return;
        }

        // 분열 연출 중
        if (_isSplitting)
        {
            _splitTimer += deltaTime;
            _shakeTimer = _splitTimer;

            if (_splitTimer >= k_SplitDuration)
            {
                _isSplitting = false;
                _shakeTimer = 0;
                SpawnPhase2Bosses();
            }
            return;
        }

        if (_shakeTimer > 0)
        {
            _shakeTimer -= deltaTime;
        }

        UpdateGameObjects(deltaTime);

        if (Input.IsKeyDown(ConsoleKey.Tab))
        {
            CycleWeapon();
        }

        // 플레이어-보스 충돌
        foreach (var boss in _bosses)
        {
            if (!boss.IsActive) continue;

            if (Overlap.IsOverlap(_player.PlayerRect(_player.CurrentDirection), boss.BossRect()))
            {
                _player.Reset();
                _isGameOver = true;
                return;
            }
        }

        // 총알-보스 충돌
        foreach (var boss in _bosses)
        {
            if (!boss.IsActive || boss.IsDead) continue;

            foreach (var bullet in _bullets)
            {
                if (!bullet.IsActive) continue;

                if (Overlap.IsOverlap(boss.BossRect(), bullet.BulletPosition))
                {
                    boss.TakeDamage(1);
                    bullet.IsActive = false;
                    RemoveGameObject(bullet);
                    _shakeTimer = 0.1f;
                    if (boss.IsDead) break; // 죽었으면 추가 총알 무시
                }
            }
        }

        // 1차전 보스 사망 → 분열 시작
        if (_phase == 1 && _bosses.Count > 0 && _bosses[0].IsDead)
        {
            var deadBoss = _bosses[0];
            _splitOrigin = deadBoss.BossPosition;
            _isSplitting = true;
            _splitTimer = 0;

            deadBoss.IsActive = false;
            RemoveGameObject(deadBoss);
            _bosses.Clear();
        }

        // 2차전 죽은 미니보스 정리
        if (_phase == 2)
        {
            foreach (var boss in _bosses)
            {
                if (boss.IsDead && boss.IsActive)
                {
                    boss.IsActive = false;
                    RemoveGameObject(boss);
                }
            }
            _bosses.RemoveAll(b => !b.IsActive);

            if (_bosses.Count == 0)
            {
                // 페이즈 번호를 먼저 올려서 다음 프레임에 재진입 방지
                _phase = 3;
                _isPhase3Intro = true;
                _phase3IntroTimer = 0;
                // 플레이어 중앙 이동
                _player.SetPosition(new Position(30, 15));
            }
        }

        // 페이즈3 전환 연출
        if (_isPhase3Intro)
        {
            _phase3IntroTimer += deltaTime;
            if (_phase3IntroTimer >= k_Phase3IntroDuration)
            {
                _isPhase3Intro = false;
                SpawnPhase3Monsters();
            }
            _bullets.RemoveAll(b => !b.IsActive);
            return;
        }

        // 3차전 - 몬스터 웨이브
        if (_phase == 3)
        {
            // 몬스터-플레이어 충돌
            foreach (var monster in _monsters)
            {
                if (!monster.IsActive) continue;

                if (Overlap.IsOverlap(_player.PlayerRect(_player.CurrentDirection), monster.MonsterRect()))
                {
                    _player.Reset();
                    _isGameOver = true;
                    _bullets.RemoveAll(b => !b.IsActive);
                    return;
                }
            }

            // 총알-몬스터 충돌
            foreach (var monster in _monsters)
            {
                if (!monster.IsActive) continue;

                foreach (var bullet in _bullets)
                {
                    if (!bullet.IsActive) continue;

                    if (Overlap.IsOverlap(monster.MonsterRect(), bullet.BulletPosition))
                    {
                        monster.IsActive = false;
                        RemoveGameObject(monster);
                        bullet.IsActive = false;
                        RemoveGameObject(bullet);
                        _monstersKilled++;
                        _player.GetGold(5);
                        break; // 이 몬스터는 죽었으니 다음 총알 체크 불필요
                    }
                }
            }

            _monsters.RemoveAll(m => !m.IsActive);

            // 전부 잡으면 승리
            if (_monstersKilled >= _totalMonstersToKill && _monsters.Count == 0)
            {
                _isVictory = true;
                _player.GetGold(100);
            }
        }

        _bullets.RemoveAll(b => !b.IsActive);
    }

    public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);

        // 체력바 (y=0~1)
        DrawBossHpBars(buffer);

        // 골드/무기 정보는 맵 밖 맨 아래 (y=29)
        buffer.WriteText(1, 29, $"Gold: {_player.Gold}G", ConsoleColor.Yellow);
        buffer.WriteText(20, 29, $"WPN: {_player.Weapon}", ConsoleColor.Gray);

        string phaseText = _phase == 1 ? "PHASE 1" : _phase == 2 ? "PHASE 2" : "FINAL";
        ConsoleColor phaseColor = _phase == 1 ? ConsoleColor.Cyan : _phase == 2 ? ConsoleColor.Magenta : ConsoleColor.Red;
        buffer.WriteText(50, 29, phaseText, phaseColor);

        if (_isSplitting)
        {
            DrawSplitEffect(buffer);
        }

        if (_isPhase3Intro)
        {
            if ((int)(_phase3IntroTimer * 6) % 2 == 0)
                buffer.WriteTextCentered(10, "!! FINAL WAVE INCOMING !!", ConsoleColor.Red);
            else
                buffer.WriteTextCentered(10, "!! FINAL WAVE INCOMING !!", ConsoleColor.DarkRed);
        }

        if (_isGameOver)
        {
            buffer.WriteTextCentered(13, "GAME OVER", ConsoleColor.Red);
            buffer.WriteTextCentered(15, "Press ENTER to Retry", ConsoleColor.White);
        }

        if (_isVictory)
        {
            buffer.WriteTextCentered(11, "* * *  V I C T O R Y  * * *", ConsoleColor.Yellow);
            buffer.WriteTextCentered(13, "KING SLIME DEFEATED!", ConsoleColor.Green);
            buffer.WriteTextCentered(15, $"Reward: 100G (Total: {_player.Gold}G)", ConsoleColor.Yellow);
            buffer.WriteTextCentered(17, "Press ENTER to Continue", ConsoleColor.White);
        }
    }

    private void DrawBossHpBars(ScreenBuffer buffer)
    {
        int fullBarWidth = 58;
        int barX = 1;

        if (_phase == 1 && _bosses.Count > 0)
        {
            var boss = _bosses[0];
            buffer.WriteTextCentered(0, "< KING SLIME >", ConsoleColor.Green);

            float ratio = (float)boss.Hp / boss.MaxHp;
            int filled = (int)(ratio * fullBarWidth);

            ConsoleColor hpColor = GetHpColor(ratio);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fullBarWidth; i++)
                sb.Append(i < filled ? '█' : '░');

            buffer.WriteText(barX, 1, sb.ToString(), hpColor);
        }
        else if (_phase == 2)
        {
            // 개별 체력바 2개로 분할 표시
            int halfBar = 27;
            int idx = 0;

            foreach (var boss in _bosses)
            {
                if (!boss.IsActive) continue;

                int startX = idx == 0 ? barX : barX + halfBar + 4;
                string label = idx == 0 ? "< SLIME A >" : "< SLIME B >";
                ConsoleColor labelColor = idx == 0 ? ConsoleColor.Green : ConsoleColor.DarkGreen;

                // 라벨 위치
                int labelX = startX + (halfBar - label.Length) / 2;
                buffer.WriteText(labelX, 0, label, labelColor);

                // 바
                float ratio = (float)boss.Hp / boss.MaxHp;
                int filled = (int)(ratio * halfBar);
                ConsoleColor hpColor = GetHpColor(ratio);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < halfBar; i++)
                    sb.Append(i < filled ? '█' : '░');

                buffer.WriteText(startX, 1, sb.ToString(), hpColor);
                idx++;
            }

            // 둘 다 죽었으면 빈 표시
            if (idx == 0)
            {
                buffer.WriteTextCentered(0, "< DEFEATED >", ConsoleColor.DarkGray);
            }
        }
        else if (_phase == 3)
        {
            int remaining = _totalMonstersToKill - _monstersKilled;
            buffer.WriteTextCentered(0, $"< FINAL WAVE - {remaining} remaining >", ConsoleColor.Red);

            float ratio = (float)_monstersKilled / _totalMonstersToKill;
            int filled = (int)(ratio * fullBarWidth);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fullBarWidth; i++)
                sb.Append(i < filled ? '█' : '░');

            buffer.WriteText(barX, 1, sb.ToString(), ConsoleColor.Magenta);
        }
    }

    private ConsoleColor GetHpColor(float ratio)
    {
        if (ratio > 0.5f) return ConsoleColor.Green;
        if (ratio > 0.25f) return ConsoleColor.Yellow;
        return ConsoleColor.Red;
    }

    private void DrawSplitEffect(ScreenBuffer buffer)
    {
        float progress = _splitTimer / k_SplitDuration;

        if ((int)(_splitTimer * 8) % 2 == 0)
            buffer.WriteTextCentered(10, "!! SLIME IS SPLITTING !!", ConsoleColor.Red);
        else
            buffer.WriteTextCentered(10, "!! SLIME IS SPLITTING !!", ConsoleColor.DarkRed);

        int cx = _splitOrigin.X + 5;
        int cy = _splitOrigin.Y + 3;
        int radius = (int)(progress * 6);

        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                int px = cx + i;
                int py = cy + j;
                if (Map.IsInBounds(px, py) && (Math.Abs(i) + Math.Abs(j) == radius))
                {
                    buffer.SetCell(px, py, '*', ConsoleColor.DarkGreen);
                }
            }
        }
    }

    private void SpawnPhase1Boss()
    {
        // Boss(Scene, Player) 생성자 → 큰 슬라임
        var boss = new Boss(this, _player);
        _bosses.Add(boss);
        AddGameObject(boss);
    }

    private void SpawnPhase2Bosses()
    {
        _phase = 2;
        _bosses.Clear();

        // GetSplitPositions()로 분열 위치 계산
        var (leftPos, rightPos) = new Boss(this, _player).GetSplitPositions();
        // 임시 보스는 사용 안 함, _splitOrigin 기준으로 직접 계산
        leftPos = new Position(_splitOrigin.X - 4, _splitOrigin.Y);
        rightPos = new Position(_splitOrigin.X + 10, _splitOrigin.Y);

        // 경계 보정
        if (leftPos.X < Map.Left) leftPos.X = Map.Left;
        if (rightPos.X + 5 > Map.Right) rightPos.X = Map.Right - 5;
        if (leftPos.Y + 3 > Map.Bottom) leftPos.Y = Map.Bottom - 3;
        if (rightPos.Y + 3 > Map.Bottom) rightPos.Y = Map.Bottom - 3;

        List<Boss> miniBosses = new List<Boss>();

        // Boss(Scene, Player, Position, List<Boss>) 생성자 → 미니 슬라임
        var mini1 = new Boss(this, _player, leftPos, miniBosses);
        var mini2 = new Boss(this, _player, rightPos, miniBosses);

        miniBosses.Add(mini1);
        miniBosses.Add(mini2);

        _bosses.Add(mini1);
        _bosses.Add(mini2);
        AddGameObject(mini1);
        AddGameObject(mini2);
    }

    public void CreateBullet(Position body, Direction dir)
    {
        var bullet = new Bullet(this, body, dir);
        _bullets.Add(bullet);
        AddGameObject(bullet);
    }

    private void SpawnPhase3Monsters()
    {
        _totalMonstersToKill = 20;
        _monstersKilled = 0;

        // 사방에서 20마리 스폰
        for (int i = 0; i < _totalMonstersToKill; i++)
        {
            var monster = new Monster(this, _player, _monsters);
            monster.Spawn(_player.PlayerRect(_player.CurrentDirection));
            _monsters.Add(monster);
            AddGameObject(monster);
        }
    }

    private void CycleWeapon()
    {
        if (_player.HasRifle && _player.HasShotgun)
        {
            if (_player.Weapon is Rifle)
                _player.SetWeapon(new ShotGun());
            else if (_player.Weapon is ShotGun)
                _player.SetWeapon(new Pistol());
            else
                _player.SetWeapon(new Rifle());
        }
        else if (_player.HasRifle)
        {
            if (_player.Weapon is Rifle)
                _player.SetWeapon(new Pistol());
            else
                _player.SetWeapon(new Rifle());
        }
        else if (_player.HasShotgun)
        {
            if (_player.Weapon is ShotGun)
                _player.SetWeapon(new Pistol());
            else
                _player.SetWeapon(new ShotGun());
        }
    }
}