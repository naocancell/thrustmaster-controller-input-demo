# 图马斯特 (Thrustmaster HOTAS A-10C) 输入与力反馈检测 Demo

本项目为图马斯特手柄/飞行摇杆（尤其是 **Thrustmaster HOTAS Warthog A-10C**）专门量身定制的输入检测与力反馈测试工具，旨在以**最快速、最直观、零门槛**的方式验证设备状态，并提供清晰的代码架构以供集成到您的主软件中。

---

## 🚀 快速开始（无需配置环境，双击即用）

1. 在文件管理器中找到工作目录下的 `index.html`：
   `c:\Users\admin\Documents\antigravity\mysterious-bell\index.html`
2. 使用任意现代浏览器（**Microsoft Edge**、**Google Chrome** 等）**双击打开**该文件。
3. **重要提示**：现代浏览器出于防指纹追踪等安全规范，在刚打开页面时手柄处于静默休眠状态。**请插好手柄后，晃动一下手柄摇杆或按下一个按键**，页面右上角将立刻识别并点亮为绿色【已连接外设】。

---

## 🎛️ 可视化功能介绍

1. **主姿态摇杆 (俯仰 & 滚转)**：
   - 采用战术座舱 HUD 准星雷达，以 60 FPS 实时绘制摇杆当前 X 轴（滚转 Roll）和 Y 轴（俯仰 Pitch）的十字准星位置与偏转矢量。
   - 实时数字指示（-1.000 ~ +1.000 高精度浮点值）。
2. **POV 苦力帽 (Hat Switch)**：
   - 8 向罗盘指针，直观呈现苦力帽开关的推向（正北、东北、正东、东南、正南、西南、正西、西北、中立位）与角度。
3. **多轴输入通道 (节流阀 / 尾舵 / 滑块)**：
   - 动态识别并列出设备上的全部模拟轴通道（双节流阀推力杆、摇杆微调滑块等），配有中点双向动态渐变条。
4. **按键发光阵列 (32+ 按键矩阵)**：
   - 实时监听手柄的所有触发按键，按战机真实功能与顺时针时钟罗盘严格对齐：
     - **DMS 数据管理 (B6~B9)**：前推(B6)、右推(B7)、后拉(B8)、左推(B9)
     - **TMS 目标管理 (B10~B13)**：前推(B10)、右推(B11)、后拉(B12)、左推(B13)
     - **CMS 干扰对抗 (B14~B18)**：前推(B14)、右推(B15)、后拉(B16)、左推(B17)、中心下按(B18)
     - **主扳机与武器释放 (B0~B5)**：一段火、二段火、武器释放大红纽、小指操纵杠杆、小指侧键、前轮转向按键
5. **力反馈与震动检测工作区 (Haptic & Vibration)**：
   - 具备独立强震动马达（低频冲击）与弱震动马达（高频颤动）百分比滑条。
   - 提供「点触测试」、「GAU-8 机炮连射脉冲震动」与「引擎大推力震动」等快捷触发按键。
   - **硬件提示**：原装 **Thrustmaster HOTAS Warthog A-10C** 飞行摇杆采用高精度霍尔磁阻弹簧基座（H.E.A.R.T 专利），**原厂硬件内部不含震动马达**。若测试台提示“未检出马达”，属于该设备的标准物理特性；若后续接入带力反馈电机的设备（如 T300RS、T248 或 eSwap 等），测试台可直接驱动电机。
6. **实时输入事件流日志**：
   - 底部内置调试黑客终端，实时记录按键的按下（包含激活度）与释放，便于调试物理开关与程序键号映射。

---

## 💻 移植与集成到主软件方案

### 方案 1：主软件为 Web / Electron / Qt WebEngine / WebView2
直接复用 `index.html` 中的轮询逻辑：
```javascript
function pollGamepadInput() {
  const gamepads = navigator.getGamepads ? navigator.getGamepads() : [];
  const gp = gamepads[0];
  if (gp) {
    const roll = gp.axes[0];  // X 轴
    const pitch = gp.axes[1]; // Y 轴
    
    // 按键检测
    gp.buttons.forEach((btn, index) => {
      if (btn.pressed) {
        console.log(`按键 #${index} 被按下!`);
      }
    });

    // 震动触发 (若设备支持)
    if (gp.vibrationActuator) {
      gp.vibrationActuator.playEffect('dual-rumble', {
        duration: 200,
        weakMagnitude: 0.5,
        strongMagnitude: 0.8
      });
    }
  }
  requestAnimationFrame(pollGamepadInput);
}
```

### 方案 2：主软件为 C# (.NET / WinForms / WPF / Unity)
我们在项目中附带了 `ThrustmasterNativeExample.cs`，通过 Windows 自带的 `winmm.dll` 进行零依赖读取：
- 导入 `joyGetPosEx` API；
- 在后台工作线程或 `Timer` 中以 20~50Hz 轮询 `JOYINFOEX` 结构体；
- 直接获取 `dwXpos`、`dwYpos`、`dwZpos`、`dwButtons`、`dwPOV` 等数据。

---

## 📁 目录文件清单

- `index.html`：零依赖单文件可视化测试工作台（双击即可在浏览器运行）。
- `ThrustmasterNativeExample.cs`：C# 原生 Windows 零依赖后台监听代码示例。
- `README.md`：说明文档。
- `task_plan.md`：任务执行记录与规划（英文）。
