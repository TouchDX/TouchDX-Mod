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

## 许可

本项目采用 [MIT License](LICENSE) 开源许可。
