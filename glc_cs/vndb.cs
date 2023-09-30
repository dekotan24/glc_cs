using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static glc_cs.General.Var;

namespace glc_cs
{
	public partial class vndb : Form
	{
		public string Title { get; set; }

		public string ImageUrl { get; set; }

		public bool RequireApply { get; set; }

		public bool SaveImage { get; set; }

		public vndb()
		{
			InitializeComponent();

			// 初期化
			Title = string.Empty;
			ImageUrl = string.Empty;
			RequireApply = false;
			SaveImage = false;
		}

		private void vndb_Load(object sender, EventArgs e)
		{
			searchText.Text = Title;
			SaveImageCheck.Checked = SaveImage;
		}

		private void searchText_KeyPress(object sender, KeyPressEventArgs e)
		{
			// Enter押下で検索を実行
			if (e.KeyChar == (char)Keys.Enter)
			{
				searchButton_Click(sender, e);
			}
		}

		private async void searchButton_Click(object sender, EventArgs e)
		{
			string keyword = searchText.Text.Trim(); // キーワードテキストボックスからキーワードを取得

			if (keyword.Length == 0)
			{
				MessageBox.Show("キーワードを入力してください。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				searchText.Focus();
				return;
			}

			searchButton.Enabled = false;

			try
			{
				// キーワードを使用して検索リクエストを送信し、JSONデータを取得
				string jsonResponse = await PerformSearchAsync(keyword);

				// JSONデータから情報を抽出して配列に格納
				SearchResult searchResult = ExtractDataFromJson(jsonResponse);

				// 画像を表示するためのフローレイアウトパネルをクリア
				flowLayoutPanel1.Controls.Clear();

				if (searchResult != null)
				{
					// 画像を追加
					foreach (var result in searchResult.results)
					{
						PictureBox pictureBox = new PictureBox();
						pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
						pictureBox.ImageLocation = result.image.url;

						// クリックイベントハンドラ
						pictureBox.Click += (s, args) =>
						{
							// 画像がクリックされたときの処理を実行
							string title = GetJapaneseTitle(result); // "ja" のタイトルを取得
							string id = result.id;
							string imageUrl = result.image.url;

							// ここで必要な処理を実行
							DialogResult dr = MessageBox.Show("タイトル：" + title + "\n\nVNID：" + id + "\n画像URL：" + imageUrl + "\n\n上記のデータで登録しますか？\n※画像取得が有効の場合は保存ダイアログが表示されます。", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
							if (dr == DialogResult.No)
							{
								return;
							}

							if (SaveImageCheck.Checked)
							{
								saveFileDialog1 = new SaveFileDialog();
								saveFileDialog1.FileName = id + "_thumb.jpg";
								saveFileDialog1.Title = "画像を保存";
								saveFileDialog1.Filter = "画像ファイル (*.jpg)|*.jpg";
								if (saveFileDialog1.ShowDialog() == DialogResult.OK)
								{
									try
									{
										WebClient wc = new WebClient();
										wc.DownloadFile(imageUrl, saveFileDialog1.FileName);
										ImageUrl = saveFileDialog1.FileName;
										wc.Dispose();
										System.Media.SystemSounds.Beep.Play();
									}
									catch (Exception ex)
									{
										WriteErrorLog("VNDBから画像の取得中にエラーが発生しました。", AppName, ex.Message);
										MessageBox.Show("VNDBから画像の取得中にエラーが発生しました。\n\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
										return;
									}
								}
							}

							// 適用
							Title = title;
							RequireApply = true;
							GC.Collect();
							this.Hide();
						};

						flowLayoutPanel1.Controls.Add(pictureBox);
					}
				}
				else
				{
					MessageBox.Show("該当データなし", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception ex)
			{
				WriteErrorLog("VNDBからデータの取得中にエラーが発生しました。", MethodBase.GetCurrentMethod().Name, ex.Message);
				MessageBox.Show("エラーが発生しました。\n\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				searchButton.Enabled = true;
			}
		}

		private string GetJapaneseTitle(ResultItem result)
		{
			// タイトル情報を検索
			string japaneseTitle = null;

			if (result.titles != null)
			{
				JArray titlesArray = (JArray)result.titles;
				var jaTitle = titlesArray.FirstOrDefault(titleInfo => titleInfo["lang"]?.ToString() == "ja");

				if (jaTitle != null)
				{
					japaneseTitle = jaTitle["title"]?.ToString();
				}
			}

			// "ja" のタイトルが見つかればその値を返し、見つからなければデフォルトのタイトルを返す
			return !string.IsNullOrEmpty(japaneseTitle) ? japaneseTitle : result.title;
		}

		private async Task<string> PerformSearchAsync(string keyword)
		{
			using (HttpClient client = new HttpClient())
			{
				string url = "https://api.vndb.org/kana/vn";

				// POSTデータを作成
				var requestData = new
				{
					filters = new object[] { "search", "=", keyword },
					fields = "title, titles.lang, titles.title, id, image.url"
				};

				// JSONデータを文字列に変換
				string jsonRequestData = JsonConvert.SerializeObject(requestData);

				// JSONデータをPOSTリクエストとして送信
				var content = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");
				var response = await client.PostAsync(url, content);

				if (response.IsSuccessStatusCode)
				{
					// レスポンスからJSONデータを取得
					string jsonResponse = await response.Content.ReadAsStringAsync();

					return jsonResponse;
				}
				else
				{
					// エラーハンドリングを行うか、エラーを示す値を返すこともできます
					throw new Exception("HTTPリクエストが失敗しました。");
				}
			}
		}

		private SearchResult ExtractDataFromJson(string jsonResponse)
		{
			try
			{
				// JSONデータをデシリアライズして SearchResult オブジェクトに変換
				SearchResult searchResult = JsonConvert.DeserializeObject<SearchResult>(jsonResponse);

				return searchResult;
			}
			catch (Exception ex)
			{
				// エラーハンドリングを行うか、エラーをログに記録することができます
				WriteErrorLog("JSONデータの解析エラー", MethodBase.GetCurrentMethod().Name, ex.Message);
				return null; // エラーが発生した場合は null を返すか、適切なエラー処理を行ってください
			}
		}
	}

	public class SearchResult
	{
		public List<ResultItem> results { get; set; }
		public bool more { get; set; }
	}

	public class ResultItem
	{
		public string title { get; set; }
		public JArray titles { get; set; }
		public string id { get; set; }
		public ImageInfo image { get; set; }
	}

	public class ImageInfo
	{
		public string url { get; set; }
	}
}
