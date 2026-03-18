using System;
using System.Threading;

namespace Framework.Engine
{
    public abstract class GameApp
    {
        private const int k_TargetFrameTime = 33; // 프레임
        private bool _isRunning;

        protected ScreenBuffer Buffer { get; private set; } //그리기

        public event GameAction GameStarted;
        public event GameAction GameStopped;

        protected GameApp(int width, int height) // 콘솔 사이즈
        {
            Buffer = new ScreenBuffer(width, height);
        }

        public void Run()
        {
            Console.CursorVisible = false;
            Console.Clear();

            _isRunning = true;
            Initialize(); // 초기화할 내용있으면 오버라이드
            GameStarted?.Invoke();

            int previousTime = Environment.TickCount;

            while (_isRunning)
            {
                int currentTime = Environment.TickCount;
                float deltaTime = (currentTime - previousTime) / 1000f;
                previousTime = currentTime;

                Input.Poll();
                Update(deltaTime);
                Buffer.Clear();
                Draw();
                Buffer.Present();

                int elapsed = Environment.TickCount - currentTime;
                int sleepTime = k_TargetFrameTime - elapsed;
                if (sleepTime > 0)
                {
                    Thread.Sleep(sleepTime);
                }
            }

            GameStopped?.Invoke();
            Console.CursorVisible = true;
            Console.ResetColor();
            Console.Clear();
        }

        protected void Quit()
        {
            _isRunning = false;
        }

        protected abstract void Initialize();
        protected abstract void Update(float deltaTime);
        protected abstract void Draw();
    }
}
