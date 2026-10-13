// ============================================================================
// 图马斯特 (Thrustmaster HOTAS A-10C) 游戏摇杆/手柄 Windows 原生读取示例
// 特点：基于 Windows 自带 winmm.dll (Joy API)，零外部依赖，任何 .NET 版本均可编译
// ============================================================================

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ThrustmasterDemo
{
    class Program
    {
        // 导入 Windows 多媒体摇杆 API
        [DllImport("winmm.dll")]
        private static extern int joyGetPosEx(int uJoyID, ref JOYINFOEX pji);

        [DllImport("winmm.dll")]
        private static extern int joyGetNumDevs();

        // 摇杆读取标志位
        private const int JOY_RETURNX = 0x00000001;
        private const int JOY_RETURNY = 0x00000002;
        private const int JOY_RETURNZ = 0x00000004;
        private const int JOY_RETURNR = 0x00000008;
        private const int JOY_RETURNU = 0x00000010;
        private const int JOY_RETURNV = 0x00000020;
        private const int JOY_RETURNPOV = 0x00000040;
        private const int JOY_RETURNBUTTONS = 0x00000080;
        private const int JOY_RETURNALL = (JOY_RETURNX | JOY_RETURNY | JOY_RETURNZ |
                                           JOY_RETURNR | JOY_RETURNU | JOY_RETURNV |
                                           JOY_RETURNPOV | JOY_RETURNBUTTONS);

        [StructLayout(LayoutKind.Sequential)]
        public struct JOYINFOEX
        {
            public int dwSize;
            public int dwFlags;
            public int dwXpos;      // X 轴 (滚转 Roll)
            public int dwYpos;      // Y 轴 (俯仰 Pitch)
            public int dwZpos;      // Z 轴 (主节流阀 Throttle)
            public int dwRpos;      // R 轴 (尾舵 Rudder)
            public int dwUpos;      // U 轴 (辅助轴 5)
            public int dwVpos;      // V 轴 (辅助轴 6)
            public int dwButtons;   // 按键状态位掩码 (支持最多 32 个按键)
            public int dwButtonNumber; // 当前按下的按键总数
            public int dwPOV;       // 苦力帽视角位置 (0 ~ 35900，或 65535 居中)
            public int dwReserved1;
            public int dwReserved2;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=================================================");
            Console.WriteLine("图马斯特 Thrustmaster A-10C 原生控制台监听测试程序");
            Console.WriteLine("=================================================");

            int totalDevices = joyGetNumDevs();
            Console.WriteLine($"系统支持的最大手柄数: {totalDevices}");

            int joystickId = 0; // 手柄索引 (通常 0 代表首选手柄)
            JOYINFOEX info = new JOYINFOEX();
            info.dwSize = Marshal.SizeOf(typeof(JOYINFOEX));
            info.dwFlags = JOY_RETURNALL;

            // 循环检测手柄是否接入
            int result = joyGetPosEx(joystickId, ref info);
            if (result != 0)
            {
                Console.WriteLine($"[提示] 手柄 ID #{joystickId} 暂未就绪或未插好，错误码: {result}");
                Console.WriteLine("请确保手柄已连接并按下手柄任意键后重试。");
            }
            else
            {
                Console.WriteLine($"[成功] 已连接手柄 ID #{joystickId}！开始实时轮询数据 (按 Ctrl+C 退出)...");
            }

            // 实时轮询主循环 (可放入主软件的后台线程或定时器中)
            while (true)
            {
                info.dwSize = Marshal.SizeOf(typeof(JOYINFOEX));
                info.dwFlags = JOY_RETURNALL;

                if (joyGetPosEx(joystickId, ref info) == 0)
                {
                    // 打印轴数值与按键状态
                    Console.SetCursorPosition(0, 7);
                    Console.WriteLine($"【姿态摇杆】 X (滚转): {info.dwXpos, 6} | Y (俯仰): {info.dwYpos, 6}       ");
                    Console.WriteLine($"【节流推力】 Z (油门): {info.dwZpos, 6} | R (尾舵): {info.dwRpos, 6}       ");
                    Console.WriteLine($"【苦力帽POV】 角度: {(info.dwPOV == 65535 ? "中立居中" : $"{info.dwPOV / 100.0}°"), 8}                 ");
                    Console.WriteLine($"【按键状态】 按下总数: {info.dwButtonNumber, 2} | 按键掩码: 0x{info.dwButtons:X8}   ");

                    // 遍历检查前 16 个按键具体哪个被按下
                    Console.Write("【按键触发】 ");
                    bool anyPressed = false;
                    for (int b = 0; b < 32; b++)
                    {
                        if ((info.dwButtons & (1 << b)) != 0)
                        {
                            Console.Write($"[B{b + 1}] ");
                            anyPressed = true;
                        }
                    }
                    if (!anyPressed) Console.Write("(无按键按下)          ");
                    Console.WriteLine("              ");
                }

                Thread.Sleep(20); // 50 Hz 轮询率
            }
        }
    }
}
