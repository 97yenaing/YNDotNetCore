using YNDotNetCore.Shared;
using YNDotNetCore.WinFormsApp.Models;
using YNDotNetCore.WinFormsApp.Query;

namespace YNDotNetCore.WinFormsApp
{
    public partial class FrmBlog : Form
    {
        private readonly DapperService _dapperService; 
        public FrmBlog()
        {   _dapperService = new DapperService(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            InitializeComponent();
        }

        private void FrmBlog_Load(object sender, EventArgs e)
        {
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtTitle.Clear(); 
            txtAuthor.Clear();
            txtContent.Clear();

            txtTitle.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                BlogModel blog = new BlogModel();
                blog.BlogTitle = txtTitle.Text.Trim();
                blog.BlogAuthor = txtAuthor.Text.Trim();
                blog.BlogContent = txtContent.Text.Trim();

                int result=_dapperService.Execute(BlogQuery.BlogCreate, blog);
                string message = result > 0 ? "saving Successful." : "Saving Failed";
                MessageBox.Show(message, "Blog",MessageBoxButtons.OK,result>0? MessageBoxIcon.Information:MessageBoxIcon.Error);
                if (result>0)
                {
                    ClearControl();
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ClearControl()
        {
            ClearControl();
        }
    }
}
