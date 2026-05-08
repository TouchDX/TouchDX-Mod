# TouchDX-Mod

这是 TouchDX 项目的 PC 端 Mod，旨在为特定游戏提供原生触摸支持，并将其桥接到 TouchDX 安卓客户端。

## 功能

*   拦截游戏原生的输入事件。
*   通过网络接收来自 TouchDX 安卓客户端的触摸数据。
*   将触摸数据模拟成游戏可以识别的原生输入事件。
*   提供低延迟的输入体验，尤其是在与 ADB 有线连接配合使用时。

## 如何使用 (玩家)

1.  前往本项目的 [Releases](https://github.com/TouchDX/TouchDX-Mod/releases) 页面。
2.  下载最新的 Release 版本压缩包（例如 `TouchDX-Mod-v1.0.0.zip`）。
3.  解压压缩包，你会得到一个 `.dll` 文件 (例如 `TouchDX.PC.Mod.dll`)。
4.  将这个 `.dll` 文件复制到你的游戏安装目录下的 `Package\Mods` 文件夹中。
5.  启动游戏。Mod 将会自动加载。

## 如何构建 (开发者)

1.  克隆本仓库到本地。
2.  使用 Visual Studio 2022 或更高版本打开 `.sln` 项目文件。
3.  确保已安装 .NET Framework 相关的开发套件。
4.  还原项目所需的 NuGet 包。
5.  选择 `Release` 配置。
6.  点击“生成” -> “生成解决方案”。
7.  编译成功后，你可以在项目的 `bin/Release` 目录下找到所需的 `.dll` 文件。

## 贡献

欢迎对本项目进行贡献！详情请参见主项目 [TouchDX/TouchDX](https://github.com/TouchDX/TouchDX)。

## 开源致谢

本项目在开发过程中，使用了以下优秀的开源项目，在此对这些项目的开发者们表示感谢：

*   **[MelonLoader](https://github.com/LavaGang/MelonLoader)**: 强大的通用游戏 Mod 加载器，为本 Mod 提供了运行环境。
*   **[Harmony](https://github.com/pardeike/Harmony)**: 极其优秀的 .NET 运行时方法修补库，使本 Mod 能够拦截和修改游戏的输入逻辑。

---

## 许可

本项目采用 [GPL-3.0 License](LICENSE) 开源许可。
