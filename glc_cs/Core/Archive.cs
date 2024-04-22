using SevenZip;
using System;
using System.IO;
using System.Reflection;
using static glc_cs.Core.Functions;
using static glc_cs.Core.Property;

namespace glc_cs
{
	internal class Archive
	{
		/// <summary>
		/// 圧縮を行います
		/// </summary>
		/// <param name="basePath">圧縮対象のパス（単一ファイルもしくはフォルダ）</param>
		/// <param name="targetPath">圧縮ファイルのパス（*.7z）</param>
		/// <param name="errorReason">エラーの理由</param>
		/// <returns>成功：True、失敗：False</returns>
		public static bool CompressArchive(string @basePath, string @targetPath, out string errorReason)
		{
			bool result = true;
			errorReason = string.Empty;

			// 7z.dllのパスを指定
			SevenZipBase.SetLibraryPath(@SevenZipDllPath);

			// ファイル
			if (!File.Exists(basePath) && !Directory.Exists(basePath))
			{
				result = false;
				errorReason = "ファイルが存在しません。";
			}
			else
			{
				// SevenZipCompressorオブジェクトを作成
				var compressor = new SevenZipCompressor();

				// 圧縮レベルを指定（Ultraは最高圧縮）
				compressor.CompressionLevel = CompressionLevel.Ultra;

				// 圧縮ファイルの形式を指定（SevenZipは7z形式）
				compressor.ArchiveFormat = OutArchiveFormat.SevenZip;

				// 圧縮
				try
				{
					compressor.CompressDirectory(basePath, targetPath);
				}
				catch (SevenZipException ex)
				{
					WriteErrorLog("圧縮中にエラーが発生しました。：" + ex.Message, MethodBase.GetCurrentMethod().Name, "BasePath:" + basePath + " / TargetPath:" + targetPath);
					result = false;
					errorReason = ex.Message;
				}
			}

			return result;
		}

		/// <summary>
		/// 解凍を行います
		/// </summary>
		/// <param name="basePath">圧縮ファイルのパス</param>
		/// <param name="targetPath">解凍先のパス</param>
		/// <param name="errorReason">エラーの理由</param>
		/// <returns>成功：True、失敗：False</returns>
		public static bool ExtractArchive(string basePath, string targetPath, out string errorReason)
		{
			bool result = true;
			errorReason = string.Empty;

			// 7z.dllのパスを指定
			SevenZipBase.SetLibraryPath(@SevenZipDllPath);

			// ファイル
			if (!File.Exists(basePath) && !Directory.Exists(basePath))
			{
				result = false;
				errorReason = "ファイルが存在しません。";
			}
			else
			{
				// SevenZipExtractorオブジェクトを作成
				var extractor = new SevenZipExtractor(basePath);

				// 解凍
				try
				{
					if (!Directory.Exists(targetPath))
					{
						Directory.CreateDirectory(targetPath);
					}

					extractor.ExtractArchive(targetPath);
				}
				catch (SevenZipException ex)
				{
					WriteErrorLog("解凍中にエラーが発生しました。（SevenZipException）：" + ex.Message, MethodBase.GetCurrentMethod().Name, "BasePath:" + basePath + " / TargetPath:" + targetPath);
					result = false;
					errorReason = ex.Message;
				}
				catch (Exception ex)
				{
					WriteErrorLog("解凍中にエラーが発生しました。（Exception）：" + ex.Message, MethodBase.GetCurrentMethod().Name, "BasePath:" + basePath + " / TargetPath:" + targetPath);
					result = false;
					errorReason = ex.Message;
				}
			}

			return result;
		}
	}
}
