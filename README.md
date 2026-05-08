# TouchDX-Mod

这是 TouchDX 项目的 PC 端 Mod，专门为 **maimai DX**（舞萌 DX）提供原生触摸和按键支持，并将其桥接到 TouchDX 安卓客户端。

## 功能

*   基于 MelonLoader 和 Harmony，拦截 maimai DX 游戏原生的 `InputManager` 和 `MeshButton` 输入事件。
*   **智能动态按键映射 (黑科技)**: Mod 会通过游戏内部函数实时获取当前状态：
    *   如果在**交互界面（选歌/菜单）**，屏幕外圈 A 区域的 8 个区块会被强制映射为 8 个外部实体物理按键，确保能够使用屏幕来正常完成 UI 交互。
    *   如果在**游玩界面（局内打歌）**，则解除物理映射，A1-A8 恢复为纯粹的触摸屏区块，确保 Slide (星星) 音符能被正确划动判定，互不干涉。
*   通过网络接收来自 TouchDX 安卓客户端的触摸和按键状态（通过端口 `4321`）。
*   将接收到的 64 位掩码数据，实时转换为游戏可以识别的屏幕触摸区域（A-E区）以及实体按键信号。
*   提供极低延迟的输入体验，消除 Windows 默认触摸手势对游戏的干扰。

## 如何使用 (玩家)

1.  前往本项目的 [Releases](https://github.com/YubaiNya/TouchDX-Mod/releases) 页面。
2.  下载最新的 Release 版本，获取 `MaiRemoteTouchMod.dll` 文件。
3.  将这个 `.dll` 文件复制到你的游戏安装目录下的 `Package\Mods` 文件夹中（需要提前安装好 MelonLoader）。
4.  启动游戏。Mod 将会自动加载并在 `4321` 端口开启监听，等待手机端连接。

## 按键映射说明

在未处于游戏判定界面时，手机端的 A 区触摸将会映射为外部按钮；在游戏中则正常触发屏幕判定。
额外提供的物理按键映射规则如下：
*   **按键 1-8**: 键盘 `W, E, D, C, X, Z, A, Q`
*   **Select**: 键盘 `3`
*   **Test**: 键盘 `F2`
*   **Service**: 键盘 `F3`
*   **Coin**: 键盘 `5`

## 如何构建 (开发者)

1.  克隆本仓库到本地。
2.  使用 Visual Studio 2022 或更高版本打开 `.csproj` 项目文件。
3.  修改项目引用，将 `Assembly-CSharp.dll`、`UnityEngine.dll` 等指向你本地游戏的 `Sinmai_Data/Managed/` 目录；`MelonLoader.dll` 等指向 `MelonLoader/net35/`。
4.  选择 `Release` 配置并生成。
5.  编译成功后即可获得所需的 `.dll` 文件。

## 💖 赞助与支持

如果您喜欢这个 Mod，欢迎通过**爱发电**请作者喝杯奶茶，支持后续的维护与更新：

<a href="https://ifdian.net/a/YubaiNya" target="_blank">
  <img src="https://img.shields.io/badge/%E7%88%B1%E5%8F%91%E7%94%B5-%E8%B5%9E%E5%8A%A9%E4%BD%9C%E8%80%85-946ce6?style=for-the-badge&logo=buy-me-a-coffee&logoColor=white" alt="赞助作者">
</a>

## 贡献

欢迎对本项目进行贡献！详情请参见主项目 [YubaiNya/TouchDX](https://github.com/YubaiNya/TouchDX)。

## 开源致谢

本项目在开发过程中，使用了以下优秀的开源项目，在此对这些项目的开发者们表示感谢：

*   **[MelonLoader](https://github.com/LavaGang/MelonLoader)**: 强大的通用游戏 Mod 加载器，为本 Mod 提供了运行环境。
*   **[Harmony](https://github.com/pardeike/Harmony)**: 极其优秀的 .NET 运行时方法修补库，使本 Mod 能够拦截和修改游戏的输入逻辑。

---

## 许可

本项目采用 [AGPL-3.0 License](LICENSE) 开源许可。
