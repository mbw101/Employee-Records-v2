using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace EmployeeRecords
{
    public partial class mainForm : Form
    {
        List<Employee> employeeDB = new List<Employee>();

        public mainForm()
        {
            InitializeComponent();
            loadDB();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Employee emp = new Employee(idInput.Text,
                fnInput.Text, 
                lnInput.Text, 
                dateInput.Text, 
                salaryInput.Text);

            employeeDB.Add(emp);

            outputLabel.Text = "new employee added";

            ClearLabels();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
           foreach (Employee emp in employeeDB)
            {
                if (emp.id == idInput.Text)
                {
                    outputLabel.Text = "employee removed";
                    employeeDB.Remove(emp);
                    ClearLabels();
                    return;
                }
            }

            outputLabel.Text = "employee not found";
            ClearLabels();
        }

        private void listButton_Click(object sender, EventArgs e)
        {
            outputLabel.Text = "";

            foreach (Employee emp in employeeDB)
            {
                outputLabel.Text += emp.id + " " + emp.firstName
                    + " " + emp.lastName
                    + " " + emp.date
                    + " " + emp.salary + "\n";
            }
        }

        private void ClearLabels()
        {
            idInput.Text = "";
            fnInput.Text = "";
            lnInput.Text = "";
            dateInput.Text = "";
            salaryInput.Text = "";
        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveDB();
        }

        public void loadDB()
        {
            string newId, newFN, newLN, newDate, newSalary;

            XmlReader reader = XmlReader.Create("Resources/EmployeeData.xml");

            // while there is another chunk to read
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    newId = reader.ReadString();

                    reader.ReadToNextSibling("firstName");
                    newFN = reader.ReadString();

                    reader.ReadToNextSibling("lastName");
                    newLN = reader.ReadString();

                    reader.ReadToNextSibling("startDate");
                    newDate = reader.ReadString();

                    reader.ReadToNextSibling("salary");
                    newSalary = reader.ReadString();

                    Employee emp = new Employee(newId, newFN, newLN, newDate, newSalary);
                    employeeDB.Add(emp);
                }
            }

            reader.Close();
        }

        public void saveDB()
        {
            XmlWriter writer = XmlWriter.Create("Resources/EmployeeData.xml", null);

            writer.WriteStartElement("Employees");

            foreach (Employee emp in employeeDB)
            {
                writer.WriteStartElement("Employee");

                writer.WriteElementString("id", emp.id);
                writer.WriteElementString("firstName", emp.firstName);
                writer.WriteElementString("lastName", emp.lastName);
                writer.WriteElementString("startDate", emp.date);
                writer.WriteElementString("salary", emp.salary);

                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.Close();
        }
    }
}
