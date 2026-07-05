# <img width="32" height="32" alt="icon_gh" src="https://github.com/user-attachments/assets/e6fc4dd6-01a4-415b-9c12-ffd037f906d9" />GLauncher（Game Launcher C# Edition）

<img width="841" height="497" alt="2026-07-06_01h37_51" src="https://github.com/user-attachments/assets/b910e0fe-1c41-46eb-ad6f-d70fe4189d93" />

## 概要
ADVゲームの一元管理（ランチャー）を目的として作成しています。<br>
趣味と勉強で開発しているので、ガバガバなところもあるかもしれません。<br>
データベースの設定方法は[Wiki](https://github.com/dekotan24/glc_cs/wiki)をご覧ください。


## 主な機能
* ゲームの登録・管理・起動（INI / SQL Server / MySQL対応）
* 起動時間トラッキング・起動回数管理
* 統計情報表示
* DLsite / VNDB / FANZAからの作品情報取得
* Discord Rich Presence連携
* リモートデスクトップ機能（WebSocket + MJPEG）
* キーボードショートカット（Enter=起動, F5=リロード, Ctrl+F=検索, Delete=削除）
* オフラインモード・データベースバックアップ
* 抽出ツール連携


## 開発環境
Visual Studio 2022 / .NET Framework 4.8.1


## 使用しているNugetパッケージ
* MySQL.Data
* Newtonsoft.Json
* AngleSharp
* NAudio
* SevenZipSharp


## DLsite Information Getter
DLsiteから作品情報を取得するアプリケーション[DLsiteInfoGetter](https://github.com/dekotan24/DLsiteInfoGetter)を使用しています。<br>
本プロジェクトファイルをコピーした場合、こちらも導入しないと動きません。


## dcon
Discord Connectorのソースコードは[dcon](https://github.com/dekotan24/dcon)リポジトリへどうぞ。


## Webビューア
MySQLデータベースに登録しているデータをブラウザで表示する[GLWeb](https://github.com/dekotan24/GLWeb/)もどうぞ。


## license
This application is developed under the MIT license.
