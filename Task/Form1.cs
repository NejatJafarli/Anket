using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task
{
    public partial class Form1 : Form
    {
        class User
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public DateTime dateTime { get; set; }
            public string FileName { get; set; }
            public override string ToString()
            {
                return $"{Name}";
            }
        }
        public Form1()
        {
            InitializeComponent();
            ListBox.SelectionMode = SelectionMode.MultiSimple;
            Timer timer = new Timer();
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (ListBox.SelectedItems.Count > 0) btnDelete.Visible = true;
            else btnDelete.Visible = false;
            if (ListBox.SelectedItems.Count == 1)
            {
                btnChange.Visible = true;
                btnChange.Location = btnAdd.Location;
                btnAdd.Visible = false;
            }
            else
            {
                btnChange.Visible = false;
                btnAdd.Visible = true;
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            User user = new User
            {
                Name = txtName.Text,
                Surname = txtSurname.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                dateTime = monthCalendar1.SelectionStart
            };
            ListBox.Items.Add(user);
            ListBox.DisplayMember = "Name";
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtJsonFileName.Text) && ListBox.SelectedItems.Count > 0)
            {
                var list = ListBox.SelectedItems;
                var TextJson = JsonSerializer.Serialize(list, new JsonSerializerOptions() { WriteIndented = true });
                using (FileStream fs = new FileStream($"{txtJsonFileName.Text}.json", FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                        sw.WriteLine(TextJson);
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!TestFileName(txtJsonFileName.Text))
            {
                var text = File.ReadAllText($"{txtJsonFileName.Text}.json");
                txtJsonFileName.Text = "";
                var JsonUser = JsonSerializer.Deserialize<List<User>>(text);
                foreach (var item in JsonUser)
                    ListBox.Items.Add(item);
            }
        }
        private bool TestFileName(in string FileName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(".");
            foreach (var item in directoryInfo.GetFiles())
            {
                if (item.Name == $"{FileName}.json")
                {
                    return false;
                }
            }
            return true;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            var ListUser = ListBox.SelectedItems[0] as User;
            txtName.Text = ListUser.Name;
            txtSurname.Text = ListUser.Surname;
            txtEmail.Text = ListUser.Email;
            txtPhone.Text = ListUser.Phone;
            monthCalendar1.SetDate(ListUser.dateTime);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            while (ListBox.SelectedItems.Count > 0)
                ListBox.Items.Remove(ListBox.SelectedItems[0]);
        }
    }
}
