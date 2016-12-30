using PensionConsultants.Data.Access;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PensionConsultants.Data.Utilities
{
    /// <summary>
    /// Represents a data access object that generates and executes CRUD operations in
    /// a given SQL Serer instance.
    /// </summary>
    public class SqlCrudOperator
    {
        public string TableName { get; private set; }
        public string PrimaryKeyName { get; private set; }
        public DataAccessComponent DataAccessComponent { get; private set; }

        public List<ColumnTuple> ColumnSet = new List<ColumnTuple>();

        public SqlCrudOperator(DataAccessComponent dataAccessComponent, string tableName)
        {
            TableName = tableName;
            DataAccessComponent = dataAccessComponent;
            PrimaryKeyName = GetPrimaryKeyName();
        }

        /// <summary>
        /// Creates a new record in the table with the specified primary key.
        /// </summary>
        /// <param name="primaryKey"></param>
        public void Create(Guid primaryKey)
        {
            Hashtable parameterList = new Hashtable();
            parameterList.Add("@PrimaryKey", primaryKey);

            foreach (ColumnTuple tuple in ColumnSet)
            {
                parameterList.Add("@" + tuple.Name, tuple.Value);
            }

            DataAccessComponent.ExecuteSqlQuery(GenerateInsertCommand(), parameterList);
        }

        /// <summary>
        /// Performs a select command on the SQL Server instance.
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="dataTable"></param>
        public void Read(Guid primaryKey, out DataTable dataTable)
        {
            Hashtable parameterList = new Hashtable();
            parameterList.Add("@PrimaryKey", primaryKey);
            dataTable = DataAccessComponent.ExecuteSqlQuery(GenerateSelectCommand(), parameterList);
        }

        /// <summary>
        /// Performs an update command on the SQL Server instance.
        /// </summary>
        /// <param name="primaryKey"></param>
        public void Update(Guid primaryKey)
        {
            Hashtable parameterList = new Hashtable();
            parameterList.Add("@PrimaryKey", primaryKey);

            foreach (ColumnTuple tuple in ColumnSet)
            {
                parameterList.Add("@" + tuple.Name, tuple.Value);
            }

            DataAccessComponent.ExecuteSqlQuery(GenerateUpdateCommand(), parameterList);
        }

        /// <summary>
        /// Preforms a delete command on the SQL Server instance.
        /// </summary>
        /// <param name="primaryKey"></param>
        public void Delete(Guid primaryKey)
        {
            Hashtable parameterList = new Hashtable();
            parameterList.Add("@PrimaryKey", primaryKey);
            DataAccessComponent.ExecuteSqlQuery(GenerateDeleteCommand(), parameterList);
        }

        /// <summary>
        /// Clears all column tuples that have been added to the instance.
        /// </summary>
        public void ClearColumns()
        {
            ColumnSet.Clear();
        }

        /// <summary>
        /// Adds a column tuple to the list of columns on which the object will perform CRUD operations.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="columnValue"></param>
        public void AddColumn(string columnName, object columnValue)
        {
            ColumnTuple tuple = new ColumnTuple();
            tuple.Name = columnName;
            tuple.Value = columnValue;
            ColumnSet.Add(tuple);
        }

        private string GenerateSelectCommand()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT * FROM <tablename> WHERE <primarykey> = @PrimaryKey");
            stringBuilder.Replace("<tablename>", this.TableName);
            stringBuilder.Replace("<primarykey>", this.PrimaryKeyName);

            return stringBuilder.ToString();
        }

        private string GenerateInsertCommand()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("INSERT INTO <tablename> (<columns>) VALUES (<values>)");
            stringBuilder.Replace("<tablename>", this.TableName);
            stringBuilder.Replace("<columns>", GenerateInsertColumns());
            stringBuilder.Replace("<values>", GenerateInsertValues());

            return stringBuilder.ToString();
        }

        private string GenerateUpdateCommand()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE <tablename> SET <columns> WHERE <primarykey> = @PrimaryKey");
            stringBuilder.Replace("<tablename>", this.TableName);
            stringBuilder.Replace("<primarykey>", this.PrimaryKeyName);
            stringBuilder.Replace("<columns>", GenerateUpdateColumns());

            return stringBuilder.ToString();
        }

        private string GenerateInsertColumns()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(this.PrimaryKeyName);

            foreach (ColumnTuple tuple in ColumnSet)
            {
                stringBuilder.Append(", " + tuple.Name);
            }

            return stringBuilder.ToString();
        }

        private string GenerateInsertValues()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("@PrimaryKey");

            foreach (ColumnTuple tuple in ColumnSet)
            {
                stringBuilder.Append(", @" + tuple.Name);
            }

            return stringBuilder.ToString();
        }

        private string GenerateUpdateColumns()
        {
            StringBuilder stringBuilder = new StringBuilder();

            int counter = 0;
            foreach (ColumnTuple tuple in ColumnSet)
            {
                if (counter == 0)
                {
                    stringBuilder.Append(tuple.Name + " = @" + tuple.Name);
                }
                else
                {
                    stringBuilder.Append(", " + tuple.Name + " = @" + tuple.Name);
                }

                counter++;
            }

            return stringBuilder.ToString();
        }

        private string GenerateDeleteCommand()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("DELETE FROM <tablename> WHERE <primarykey> = @PrimaryKey");
            stringBuilder.Replace("<tablename>", this.TableName);
            stringBuilder.Replace("<primarykey>", this.PrimaryKeyName);

            return stringBuilder.ToString();
        }

        private string GetPrimaryKeyName()
        {
            string command = @"
                SELECT COLUMN_NAME
                FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                WHERE
                    OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + CONSTRAINT_NAME), 'IsPrimaryKey') = 1
                    AND TABLE_NAME = @TableName ";

            Hashtable parameterList = new Hashtable();
            parameterList.Add("@TableName", this.TableName);
            DataTable dataTable = DataAccessComponent.ExecuteSqlQuery(command, parameterList);

            return dataTable.Rows[0]["COLUMN_NAME"].ToString();
        }
    }
}
