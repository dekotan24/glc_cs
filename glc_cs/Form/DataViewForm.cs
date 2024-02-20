using System.Data;
using System.Windows.Forms;

namespace glc_cs
{
	public partial class DataViewForm : Form
	{
		public DataViewForm(DataTable dt)
		{
			InitializeComponent();

			dataGridView1.DataSource = dt;
		}
	}
}
