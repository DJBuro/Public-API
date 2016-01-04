using System;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using CodeSmith.Engine;
using CodeSmith.CustomProperties;
using SchemaExplorer;

// ===========================================================================================
// TODO add more comments to these support routines
// TODO integrate use of MAPs for conversions
// ===========================================================================================

namespace CodeSmith.BaseTemplates
{
	public class WRCommon: CodeTemplate 
	{
		#region Enums
		public enum WRDatabase {Ignore, Hub, MA, CStore, Logins, Directories, EmailGateway, Restaurant, RestaurantHandbook, GrocerJobs, Grocer, Corporate, Voting, IWC, PriceIndex, FiftyBestRestuarants, DrinksInternational, EmailStats, XExperimentJRH, ProductPromo, FormServer, MART, Exceptions, WilliamReedParty, Survey, MartUtility};
		public enum WRStoredProcPermissions {Ignore, Norbert, MAWebUser, RunStoredProcs, CorpSiteWebUser, grocercoukwebuser, trapdoor, webuser, wrhub, WRDirectoriesUser};
		public enum WRStoredProcReturnType {Ignore, NoReturn, SqlDataReader, DataSet, Scalar, OutputParameters};
		#endregion

		#region Privates
		private StringCollection _droppedProcedureNames = new StringCollection();
		private StringCollection _generatedProcedureNames = new StringCollection();
		private TableSchema _sourceTable;
		private string _bindingTable1 = "";
		private string _bindingTable2 = "";
		private string _bindingTable3 = "";
		#endregion
		
		#region General
		public void ShowValidationError(string errorMessage)
		{
			Trace.WriteLine(errorMessage);
			MessageBox.Show(errorMessage, "CodeSmith Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}
		
		public bool ContainsTextFields(ColumnSchemaCollection columns)
		{
			for (int i = 0; i<columns.Count; i++)
			{
				switch (columns[i].NativeType.ToLower())
				{
					case "text": return true;
					case "ntext": return true;
					case "image": return true;
				}
			}
			
			return false;
		}
		#endregion

		#region Public Properties
		[Category("Source Data"), Description("Stored procedures will be generated for this table")]
		public virtual TableSchema SourceTable
		{
			get
			{
				return _sourceTable;
			}
			set
			{
				_sourceTable = value;
			}
		}
		#endregion
		
		#region C# Code Generation Helpers
		public void GenerateCSharpVariable(ColumnSchema column, bool isFirst, bool isLast)
		{
			Response.Write(StringUtil.ToCamelCase(column.Name));
			if (!isLast) Response.Write(", ");
		}

		public void GenerateCSharpVariable(ParameterSchema parameter, bool isFirst, bool isLast)
		{
			Response.Write(StringUtil.ToCamelCase(parameter.Name.Remove(0, 1))); // remove "@" from front of parameter name
			if (!isLast) Response.Write(", ");
		}

		public void GenerateCSharpParameter(ColumnSchema column, bool isFirst, bool isLast)
		{
			Response.Write(GetCSharpVariableType(column.DataType));
			Response.Write(" ");
			Response.Write(StringUtil.ToCamelCase(column.Name));
			if (!isLast) Response.Write(", ");
		}

		public void GenerateCSharpParameter(ParameterSchema parameter, bool isFirst, bool isLast)
		{
			Response.Write(GetCSharpVariableType(parameter.DataType));
			Response.Write(" ");
			Response.Write(StringUtil.ToCamelCase(parameter.Name.Remove(0, 1)));
			if (!isLast) Response.Write(", ");
		}

		public void GenerateCSharpXMLDescriptor(ColumnSchema column, int indentLevel)
		{
			GenerateIndent(indentLevel);
			Response.Write("/// <param name=\"");
			Response.Write(StringUtil.ToCamelCase(column.Name));
			Response.Write("\"></param>");
			Response.WriteLine("");
		}

		public void GenerateCSharpXMLDescriptor(ParameterSchema parameter, int indentLevel)
		{
			GenerateIndent(indentLevel);
			Response.Write("/// <param name=\"");
			Response.Write(StringUtil.ToCamelCase(parameter.Name.Remove(0, 1)));
			Response.Write("\"></param>");
			Response.WriteLine("");
		}

		public void GenerateCSharpVariables(ColumnSchemaCollection columns, bool includeTrailingComma, ColumnSchema excludeColumn)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(columns, excludeColumn);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				GenerateCSharpVariable(filteredColumns[i], i == 0, i == filteredColumns.Count - 1 && !includeTrailingComma);
			}
		}

		public void GenerateCSharpVariables(ParameterSchemaCollection parameters, bool includeTrailingComma)
		{
			for (int i = 0; i < parameters.Count; i++)
			{
				GenerateCSharpVariable(parameters[i], i == 0, i == parameters.Count - 1 && !includeTrailingComma);
			}
		}

		public void GenerateCSharpVariablesPrimaryKeys(PrimaryKeySchema primaryKey, bool includeTrailingComma, ColumnSchema excludeColumn)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(primaryKey.MemberColumns, excludeColumn);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				GenerateCSharpVariable(filteredColumns[i], i == 0, i == filteredColumns.Count - 1 && !includeTrailingComma);
			}
		}

		public void GenerateCSharpParameters(ColumnSchemaCollection columns, bool includeTrailingComma, ColumnSchema excludeColumn)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(columns, excludeColumn);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				GenerateCSharpParameter(filteredColumns[i], i == 0, i == filteredColumns.Count - 1 && !includeTrailingComma);
			}
		}

		public void GenerateCSharpParameters(ParameterSchemaCollection parameters, bool includeTrailingComma)
		{
			for (int i = 0; i < parameters.Count; i++)
			{
				GenerateCSharpParameter(parameters[i], i == 0, i == parameters.Count - 1 && !includeTrailingComma);
			}
		}

		public void GenerateCSharpXMLDescriptors(ColumnSchemaCollection columns, int indentLevel, ColumnSchema excludeColumn)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(columns, excludeColumn);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				GenerateCSharpXMLDescriptor(filteredColumns[i], indentLevel);
			}
		}

		public void GenerateCSharpXMLDescriptors(ParameterSchemaCollection parameters, int indentLevel)
		{
			for (int i = 0; i < parameters.Count; i++)
			{
				GenerateCSharpXMLDescriptor(parameters[i], indentLevel);
			}
		}

		public void GenerateCSharpParametersPrimaryKeys(PrimaryKeySchema primaryKey, bool includeTrailingComma, ColumnSchema excludeColumn)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(primaryKey.MemberColumns, excludeColumn);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				GenerateCSharpParameter(filteredColumns[i], i == 0, i == filteredColumns.Count - 1 && !includeTrailingComma);
			}
		}

		public void GenerateCSharpXMLDescriptorsPrimaryKeys(PrimaryKeySchema primaryKey, int indentLevel, ColumnSchema excludeColumn)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(primaryKey.MemberColumns, excludeColumn);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				GenerateCSharpXMLDescriptor(filteredColumns[i], indentLevel);
			}
		}
		#endregion
		
		#region T-SQL Code Generation Helpers
		public string GetTableOwner()
		{
			return GetTableOwner(true);
		}

		public string GetTableOwner(bool includeDot)
		{
			if (_sourceTable.Owner.Length > 0)
			{
				if (includeDot)
				{
					return "[" + _sourceTable.Owner + "].";
				}
				else
				{
					return "[" + _sourceTable.Owner + "]";
				}
			}
			else
			{
				return "";
			}
		}

		public void GenerateDropStatement(string procedureName)
		{
			// check to see if this procedure has already been dropped.
			if (!_droppedProcedureNames.Contains(procedureName))
			{
				Response.WriteLine("IF OBJECT_ID(N'{0}') IS NOT NULL", procedureName);
				GenerateIndent(1);
				Response.WriteLine("DROP PROCEDURE {0}", procedureName);
				Response.WriteLine("");

				// add this procedure to the list of dropped procedures
				_droppedProcedureNames.Add(procedureName);
			}
		}

		public void GenerateProcedureHeader(string procedureName)
		{
			Response.WriteLine("--region {0}", procedureName);
			Response.WriteLine("");
			Response.WriteLine("----------------------------------------------------");
			Response.WriteLine("-- Template:        {0} (CodeSmith)", this.CodeTemplateInfo.FileName);
			Response.WriteLine("-- Procedure Name:  {0}", procedureName);
			
			// ========================================================================
			// NOTE: unable to put the VAULT FileName, Author, Version and History tags 
			// here as they get filled when this file WRCommon.cst.cs is checked in and 
			// checked out of vault.
			// ========================================================================

 			Response.WriteLine("----------------------------------------------------");
		}

		public void GenerateProcedureFooter(string procedureName)
		{
			Response.WriteLine("--endregion");
			Response.WriteLine("");
			Response.WriteLine("GO");
			Response.WriteLine("");
		}

		public void GenerateIndent(int indentLevel)
		{
			for (int i = 0; i < indentLevel; i++)
			{
				Response.Write('\t');
			}
		}

		public void GenerateParameter(ColumnSchema column, int indentLevel, bool isFirst, bool isLast)
		{
			GenerateParameter(column, indentLevel, isFirst, isLast, false);
		}

		public void GenerateParameter(ColumnSchema column, int indentLevel, bool isFirst, bool isLast, bool isOutput)
		{
			GenerateIndent(indentLevel);
			Response.Write(GetSqlParameterStatement(column, isOutput));
			if (!isLast) Response.Write(",");
			if (indentLevel >= 0)
			{
				Response.WriteLine("");
			}
			else if (!isLast)
			{
				Response.Write(" ");
			}
		}

		public void GenerateParameters(ColumnSchemaCollection columns, int indentLevel, StringCollection excludedColumns)
		{
			GenerateParameters(columns, indentLevel, false, excludedColumns);
		}

		public void GenerateParameters(ColumnSchemaCollection columns, int indentLevel, bool includeTrailingComma, StringCollection excludedColumns)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(columns, excludedColumns);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				GenerateParameter(filteredColumns[i], indentLevel, i == 0, i == filteredColumns.Count - 1 && !includeTrailingComma);
			}
		}

		public void GenerateParameters(ColumnSchemaCollection columns, int indentLevel, bool includeTrailingComma, bool excludePrimaryKeyColumn, StringCollection excludedColumns)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(columns, excludedColumns);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				if (excludePrimaryKeyColumn)
				{
					if (filteredColumns[i].Name != _sourceTable.PrimaryKey.MemberColumns[0].Name)
						GenerateParameter(filteredColumns[i], indentLevel, i == 0, i == filteredColumns.Count - 1 && !includeTrailingComma);
				}
				else
					GenerateParameter(filteredColumns[i], indentLevel, i == 0, i == filteredColumns.Count - 1 && !includeTrailingComma);
			}
		}

		public void GenerateParameters(ColumnSchemaCollection columns, int indentLevel, bool includeTrailingComma, bool excludePrimaryKeyColumn, StringCollection excludedColumns, bool isOuput)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(columns, excludedColumns);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				if (excludePrimaryKeyColumn)
				{
					if (filteredColumns[i].Name != _sourceTable.PrimaryKey.MemberColumns[0].Name)
						GenerateParameter(filteredColumns[i], indentLevel, i == 0, i == filteredColumns.Count - 1 && !includeTrailingComma, isOuput);
				}
				else
					GenerateParameter(filteredColumns[i], indentLevel, i == 0, i == filteredColumns.Count - 1 && !includeTrailingComma, isOuput);
			}
		}

		public void GenerateColumn(ColumnSchema column, int indentLevel, bool isFirst, bool isLast)
		{
			GenerateIndent(indentLevel);
			Response.Write("[");
			Response.Write(column.Name);
			Response.Write("]");
			if (!isLast) Response.Write(",");
			if (indentLevel >= 0)
			{
				Response.WriteLine("");
			}
			else if (!isLast)
			{
				Response.Write(" ");
			}
		}

		public void GenerateColumns(ColumnSchemaCollection columns, int indentLevel, StringCollection excludedColumns)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(columns, excludedColumns);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				GenerateColumn(filteredColumns[i], indentLevel, i == 0, i == filteredColumns.Count - 1);
			}
		}

		public void GenerateColumns(ColumnSchemaCollection columns, int indentLevel, bool excludePrimaryKeyColumn, StringCollection excludedColumns)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(columns, excludedColumns);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				if (excludePrimaryKeyColumn)
				{
					if (filteredColumns[i].Name != _sourceTable.PrimaryKey.MemberColumns[0].Name)
						GenerateColumn(filteredColumns[i], indentLevel, i == 0, i == filteredColumns.Count - 1);
				}
				else
					GenerateColumn(filteredColumns[i], indentLevel, i == 0, i == filteredColumns.Count - 1);
			}
		}

		public void GenerateUpdate(ColumnSchema column, int indentLevel, bool isFirst, bool isLast)
		{
			GenerateIndent(indentLevel);
			Response.Write("[");
			Response.Write(column.Name);
			Response.Write("] = @");
			Response.Write(column.Name);
			if (!isLast) Response.Write(",");
			if (indentLevel >= 0)
			{
				Response.WriteLine("");
			}
			else if (!isLast)
			{
				Response.Write(" ");
			}
		}

		public void GenerateUpdates(ColumnSchemaCollection columns, int indentLevel, StringCollection excludedColumns, StringCollection readOnlyColumns)
		{
			ColumnSchemaCollection filteredColumns = FilterReadOnlyAndExcludedColumns(columns, excludedColumns, readOnlyColumns);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				GenerateUpdate(filteredColumns[i], indentLevel, i == 0, i == filteredColumns.Count - 1);
			}
		}

		public void GenerateOutputColumn(ColumnSchema column, int indentLevel, bool isFirst, bool isLast)
		{
			GenerateIndent(indentLevel);
			Response.Write("@");
			Response.Write(column.Name);
			Response.Write(" = [");
			Response.Write(column.Name);
			Response.Write("]");
			if (!isLast) Response.Write(",");
			if (indentLevel >= 0)
			{
				Response.WriteLine("");
			}
			else if (!isLast)
			{
				Response.Write(" ");
			}
		}

		public void GenerateOutputColumns(ColumnSchemaCollection columns, int indentLevel, StringCollection excludedColumns)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(columns, excludedColumns);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				GenerateOutputColumn(filteredColumns[i], indentLevel, i == 0, i == filteredColumns.Count - 1);
			}
		}

		public void GenerateCondition(ColumnSchema column, int indentLevel, bool isFirst, bool isLast)
		{
			GenerateIndent(indentLevel);
			if (!isFirst) Response.Write("AND ");
			Response.Write("[");
			Response.Write(column.Name);
			Response.Write("] = @");
			Response.Write(column.Name);
			if (indentLevel >= 0)
			{
				Response.WriteLine("");
			}
			else if (!isLast)
			{
				Response.Write(" ");
			}
		}

		public void GenerateConditions(ColumnSchemaCollection columns, int indentLevel, StringCollection excludedColumns)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(columns, excludedColumns);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				GenerateCondition(filteredColumns[i], indentLevel, i == 0, i == filteredColumns.Count - 1);
			}
		}

		public void GenerateVariable(ColumnSchema column, int indentLevel, bool isFirst, bool isLast)
		{
			GenerateIndent(indentLevel);
			Response.Write("@");
			Response.Write(column.Name);
			if (!isLast) Response.Write(",");
			if (indentLevel >= 0)
			{
				Response.WriteLine("");
			}
			else if (!isLast)
			{
				Response.Write(" ");
			}
		}

		public void GenerateVariables(ColumnSchemaCollection columns, int indentLevel, StringCollection excludedColumns)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(columns, excludedColumns);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				GenerateVariable(filteredColumns[i], indentLevel, i == 0, i == filteredColumns.Count - 1);
			}
		}

		public void GenerateVariables(ColumnSchemaCollection columns, int indentLevel, bool excludePrimaryKeyColumn, StringCollection excludedColumns)
		{
			ColumnSchemaCollection filteredColumns = FilterExcludedColumns(columns, excludedColumns);
			for (int i = 0; i < filteredColumns.Count; i++)
			{
				if (excludePrimaryKeyColumn)
				{
					if (filteredColumns[i].Name != _sourceTable.PrimaryKey.MemberColumns[0].Name)
						GenerateVariable(filteredColumns[i], indentLevel, i == 0, i == filteredColumns.Count - 1);
				}
				else
					GenerateVariable(filteredColumns[i], indentLevel, i == 0, i == filteredColumns.Count - 1);
			}
		}

		public void GenerateOrderByClause(string orderByExpression)
		{
			if (orderByExpression != null && orderByExpression.Trim().Length > 0)
			{
				GenerateIndent(1);
				Response.WriteLine("ORDER BY");
				GenerateIndent(2);
				Response.WriteLine(orderByExpression);
			}
		}

		public ColumnSchemaCollection FilterReadOnlyColumns(ColumnSchemaCollection columns, StringCollection readOnlyColumns)
		{
			ColumnSchemaCollection filteredColumns = new ColumnSchemaCollection();

			for (int i = 0; i < columns.Count; i++)
			{
				if (!ColumnIsReadOnly(columns[i], readOnlyColumns)) filteredColumns.Add(columns[i]);
			}

			return filteredColumns;
		}

		public ColumnSchemaCollection FilterExcludedColumns(ColumnSchemaCollection columns, StringCollection excludedColumns)
		{
			ColumnSchemaCollection filteredColumns = new ColumnSchemaCollection();

			for (int i = 0; i < columns.Count; i++)
			{
				if (!ColumnIsExcluded(columns[i], excludedColumns)) filteredColumns.Add(columns[i]);
			}

			return filteredColumns;
		}

		public ColumnSchemaCollection FilterExcludedColumns(ColumnSchemaCollection columns, ColumnSchema excludedColumn)
		{
			ColumnSchemaCollection filteredColumns = new ColumnSchemaCollection();

			for (int i = 0; i < columns.Count; i++)
			{
				if ((excludedColumn == null) || 
					((excludedColumn != null) && (columns[i].Name != excludedColumn.Name)))
					filteredColumns.Add(columns[i]);
			}

			return filteredColumns;
		}

		public ColumnSchemaCollection FilterReadOnlyAndExcludedColumns(ColumnSchemaCollection columns, StringCollection excludedColumns, StringCollection readOnlyColumns)
		{
			ColumnSchemaCollection filteredColumns = new ColumnSchemaCollection();

			for (int i = 0; i < columns.Count; i++)
			{
				if (!ColumnIsExcludedOrReadOnly(columns[i], excludedColumns, readOnlyColumns)) filteredColumns.Add(columns[i]);
			}

			return filteredColumns;
		}

		private Regex excludedColumnRegex = null;

		public bool ColumnIsExcluded(ColumnSchema column, StringCollection excludedColumns)
		{
			if (column.IsPrimaryKeyMember) return false;

			if (excludedColumnRegex == null)
			{
				if (excludedColumns != null && excludedColumns.Count > 0)
				{
					string excluded = String.Empty;
					for (int i = 0; i < excludedColumns.Count; i++)
					{
						if (excludedColumns[i].Trim().Length > 0)
						{
							excluded += "(" + Regex.Escape(excludedColumns[i]).Replace("\\*", ".*?") + ")|";
						}
					}

					if (excluded.Length > 0)
					{
						excluded = excluded.Substring(0, excluded.Length - 1);
						excludedColumnRegex = new Regex(excluded, RegexOptions.IgnoreCase);
					}
				}
			}

			if (excludedColumnRegex != null && excludedColumnRegex.IsMatch(column.Name)) return true;

			return false;
		}

		private Regex readOnlyColumnRegex = null;

		public bool ColumnIsReadOnly(ColumnSchema column, StringCollection readOnlyColumns)
		{
			if (column.IsPrimaryKeyMember) return false;

			if (readOnlyColumnRegex == null)
			{
				if (readOnlyColumns != null && readOnlyColumns.Count > 0)
				{
					string readOnly = String.Empty;
					for (int i = 0; i < readOnlyColumns.Count; i++)
					{
						if (readOnlyColumns[i].Trim().Length > 0)
						{
							readOnly += "(" + Regex.Escape(readOnlyColumns[i]).Replace("\\*", ".*?") + ")|";
						}
					}

					if (readOnly.Length > 0)
					{
						readOnly = readOnly.Substring(0, readOnly.Length - 1);
						readOnlyColumnRegex = new Regex(readOnly, RegexOptions.IgnoreCase);
					}
				}
			}

			if (readOnlyColumnRegex != null && readOnlyColumnRegex.IsMatch(column.Name)) return true;

			return false;
		}

		public bool ColumnIsExcludedOrReadOnly(ColumnSchema column, StringCollection excludedColumns, StringCollection readOnlyColumns)
		{
			return ColumnIsExcluded(column, excludedColumns) || ColumnIsReadOnly(column, readOnlyColumns);
		}
		#endregion

		#region Procedure Naming
		public string GetAddProcedureName(string procedurePrefix, string tablePrefix)
		{
			return String.Format("{0}[{1}{2}Add]", GetTableOwner(), procedurePrefix, GetEntityName(false, tablePrefix));
		}

		public string GetUpdateProcedureName(string procedurePrefix, string tablePrefix)
		{
			return String.Format("{0}[{1}{2}Update]", GetTableOwner(), procedurePrefix, GetEntityName(false, tablePrefix));
		}

		public string GetAddUpdateProcedureName(string procedurePrefix, string tablePrefix)
		{
			return String.Format("{0}[{1}{2}AddUpdate]", GetTableOwner(), procedurePrefix, GetEntityName(false, tablePrefix));
		}

		public string GetDeleteProcedureName(string procedurePrefix, string tablePrefix)
		{
			return String.Format("{0}[{1}{2}Delete]", GetTableOwner(), procedurePrefix, GetEntityName(false, tablePrefix));
		}

		public string GetSelectProcedureName(string procedurePrefix, string tablePrefix)
		{
			return String.Format("{0}[{1}{2}Get]", GetTableOwner(), procedurePrefix, GetEntityName(true, tablePrefix));
		}

		public string GetSelectRowProcedureName(string procedurePrefix, string tablePrefix)
		{
			return String.Format("{0}[{1}{2}GetRow]", GetTableOwner(), procedurePrefix, GetEntityName(false, tablePrefix));
		}

		public string GetBindingTableAddProcedureName(string procedurePrefix, TableSchema bindingTable)
		{
			return String.Format("{0}[{1}{2}Add]", GetTableOwner(), procedurePrefix, GetBindingTableEntityName(bindingTable));
		}
		
		public string GetBindingTableAddUpdateProcedureName(string procedurePrefix, TableSchema bindingTable)
		{
			return String.Format("{0}[{1}{2}AddUpdate]", GetTableOwner(), procedurePrefix, GetBindingTableEntityName(bindingTable));
		}

		public string GetBindingTableRemoveProcedureName(string procedurePrefix, TableSchema bindingTable)
		{
			return String.Format("{0}[{1}{2}Remove]", GetTableOwner(), procedurePrefix, GetBindingTableEntityName(bindingTable));
		}

		public string GetBindingTableGetProcedureName1(string procedurePrefix, TableSchema bindingTable)
		{
			return String.Format("{0}[{1}{2}GetOn{3}]", GetTableOwner(), procedurePrefix, GetBindingTableEntityName(bindingTable), bindingTable.PrimaryKey.MemberColumns[0].Name );
		}

		public string GetBindingTableGetProcedureName2(string procedurePrefix, TableSchema bindingTable)
		{
			return String.Format("{0}[{1}{2}GetOn{3}]", GetTableOwner(), procedurePrefix, GetBindingTableEntityName(bindingTable), bindingTable.PrimaryKey.MemberColumns[1].Name );
		}

		public string GetBindingTableRemoveAllProcedureName1(string procedurePrefix, TableSchema bindingTable)
		{
			return String.Format("{0}[{1}{2}RemoveAllOn{3}]", GetTableOwner(), procedurePrefix, GetBindingTableEntityName(bindingTable), bindingTable.PrimaryKey.MemberColumns[0].Name );
		}

		public string GetBindingTableRemoveAllProcedureName2(string procedurePrefix, TableSchema bindingTable)
		{
			return String.Format("{0}[{1}{2}RemoveAllOn{3}]", GetTableOwner(), procedurePrefix, GetBindingTableEntityName(bindingTable), bindingTable.PrimaryKey.MemberColumns[1].Name );
		}

		public string GetBindingTableEntityName(TableSchema bindingTable)
		{
			// remove the first 2 characters of the table name (these should be the prefix "_b")
			return bindingTable.Name.Substring(2);
		}

		public string GetEntityName(bool plural, string tablePrefix)
		{
			string entityName = _sourceTable.Name;

			if (entityName.StartsWith(tablePrefix))
			{
				entityName = entityName.Substring(tablePrefix.Length);
			}

            if (plural)
            {
                entityName = StringUtil.ToPlural(entityName);
            }
            else
            {
                entityName = StringUtil.ToSingular(entityName);
            }

            return entityName;
		}

		public string GetBySuffix(ColumnSchemaCollection columns)
		{
			System.Text.StringBuilder bySuffix = new System.Text.StringBuilder();
			for (int i = 0; i < columns.Count; i++)
			{
				if (i > 0) bySuffix.Append("And");
				bySuffix.Append(columns[i].Name);
			}

			return bySuffix.ToString();
		}
		#endregion
		
		#region Class Naming
		public string GetDALClassName(string tablePrefix)
		{
			return GetEntityName(false, tablePrefix) + "DALbase";
		}
		
		public string GetDALDerivativeClassName(string tablePrefix)
		{
			return GetEntityName(false, tablePrefix) + "DAL";
		}
		
		public string GetBLLClassName(string tablePrefix)
		{
			return GetEntityName(false, tablePrefix) + "BLLbase";
		}
		
		public string GetBLLDerivativeClassName(string tablePrefix)
		{
			return GetEntityName(false, tablePrefix) + "BLL";
		}
		#endregion
		
		#region File Naming
		public string GetDALFileName(string tablePrefix)
		{
			return GetEntityName(false, tablePrefix) + "DALbase.cs";
		}

		public string GetDALDerivativeFileName(string tablePrefix)
		{
			return GetEntityName(false, tablePrefix) + "DAL.cs";
		}

		public string GetBLLFileName(string tablePrefix)
		{
			return GetEntityName(false, tablePrefix) + "BLLbase.cs";
		}

		public string GetBLLDerivativeFileName(string tablePrefix)
		{
			return GetEntityName(false, tablePrefix) + "BLL.cs";
		}

		public string GetScriptFileName(string tablePrefix)
		{
			return GetEntityName(false, tablePrefix) + "_StoredProcedures.sql";
		}
		#endregion
		
		#region Data Helper Routines
		public string GetParamAttributes(SchemaExplorer.ColumnSchema column)
		{
			string returnValue = "\"@" + column.Name + "\"";
			
			switch (column.DataType)
			{
				case DbType.AnsiString: returnValue = returnValue + ", SqlDbType.VarChar, " + column.Size.ToString(); break;
				case DbType.AnsiStringFixedLength:  returnValue = returnValue + ", SqlDbType.Char, " + column.Size.ToString(); break;
				case DbType.Binary: returnValue = returnValue + ", SqlDbType.VarBinary"; break;
				case DbType.Boolean: returnValue = returnValue + ", SqlDbType.Bit"; break;
				case DbType.Byte: returnValue = returnValue + ", SqlDbType.TinyInt"; break;
				case DbType.Currency: returnValue = returnValue + ", SqlDbType.Money"; break;
				case DbType.DateTime: returnValue = returnValue + ", SqlDbType.DateTime"; break;
				case DbType.Decimal: returnValue = returnValue + ", SqlDbType.Decimal"; break;
				case DbType.Double: returnValue = returnValue + ", SqlDbType.Float"; break;
				case DbType.Guid: returnValue = returnValue + ", SqlDbType.UniqueIdentifier"; break;
				case DbType.Int16: returnValue = returnValue + ", SqlDbType.SmallInt"; break;
				case DbType.Int32: returnValue = returnValue + ", SqlDbType.Int"; break;
				case DbType.Int64: returnValue = returnValue + ", SqlDbType.BigInt"; break;
				case DbType.Object: returnValue = returnValue + ", SqlDbType.Variant"; break;
				case DbType.Single: returnValue = returnValue + ", SqlDbType.Real"; break;
				case DbType.String: returnValue = returnValue + ", SqlDbType.NVarChar, " + column.Size.ToString(); break;
				case DbType.StringFixedLength: returnValue = returnValue + ", SqlDbType.NChar, " + column.Size.ToString(); break;
				default:
				{
					return "__UNSUPPORTED__" + column.DataType.ToString();
				}
			}
			
			return returnValue;
		}
		
		public string GetParamAttributes(SchemaExplorer.ParameterSchema parameter)
		{
			string returnValue = "\"" + parameter.Name + "\"";
			
			switch (parameter.DataType)
			{
				case DbType.AnsiString: returnValue = returnValue + ", SqlDbType.VarChar, " + parameter.Size.ToString(); break;
				case DbType.AnsiStringFixedLength:  returnValue = returnValue + ", SqlDbType.Char, " + parameter.Size.ToString(); break;
				case DbType.Binary: returnValue = returnValue + ", SqlDbType.VarBinary"; break;
				case DbType.Boolean: returnValue = returnValue + ", SqlDbType.Bit"; break;
				case DbType.Byte: returnValue = returnValue + ", SqlDbType.TinyInt"; break;
				case DbType.Currency: returnValue = returnValue + ", SqlDbType.Money"; break;
				case DbType.DateTime: returnValue = returnValue + ", SqlDbType.DateTime"; break;
				case DbType.Decimal: returnValue = returnValue + ", SqlDbType.Decimal"; break;
				case DbType.Double: returnValue = returnValue + ", SqlDbType.Float"; break;
				case DbType.Guid: returnValue = returnValue + ", SqlDbType.UniqueIdentifier"; break;
				case DbType.Int16: returnValue = returnValue + ", SqlDbType.SmallInt"; break;
				case DbType.Int32: returnValue = returnValue + ", SqlDbType.Int"; break;
				case DbType.Int64: returnValue = returnValue + ", SqlDbType.BigInt"; break;
				case DbType.Object: returnValue = returnValue + ", SqlDbType.Variant"; break;
				case DbType.Single: returnValue = returnValue + ", SqlDbType.Real"; break;
				case DbType.String: returnValue = returnValue + ", SqlDbType.NVarChar, " + parameter.Size.ToString(); break;
				case DbType.StringFixedLength: returnValue = returnValue + ", SqlDbType.NChar, " + parameter.Size.ToString(); break;
				default:
				{
					return "__UNSUPPORTED__" + parameter.DataType.ToString();
				}
			}
			
			return returnValue;
		}

		public string GetParamAttributes(SchemaExplorer.PrimaryKeySchema primaryKey)
		{
			ColumnSchema column = primaryKey.MemberColumns[0]; // assumes only one primary key in table
			string returnValue = "\"@" + column.Name + "\"";
			
			switch (column.DataType)
			{
				case DbType.AnsiString: returnValue = returnValue + ", SqlDbType.VarChar, " + column.Size.ToString(); break;
				case DbType.AnsiStringFixedLength:  returnValue = returnValue + ", SqlDbType.Char, " + column.Size.ToString(); break;
				case DbType.Binary: returnValue = returnValue + ", SqlDbType.VarBinary"; break;
				case DbType.Boolean: returnValue = returnValue + ", SqlDbType.Bit"; break;
				case DbType.Byte: returnValue = returnValue + ", SqlDbType.TinyInt"; break;
				case DbType.Currency: returnValue = returnValue + ", SqlDbType.Money"; break;
				case DbType.DateTime: returnValue = returnValue + ", SqlDbType.DateTime"; break;
				case DbType.Decimal: returnValue = returnValue + ", SqlDbType.Decimal"; break;
				case DbType.Double: returnValue = returnValue + ", SqlDbType.Float"; break;
				case DbType.Guid: returnValue = returnValue + ", SqlDbType.UniqueIdentifier"; break;
				case DbType.Int16: returnValue = returnValue + ", SqlDbType.SmallInt"; break;
				case DbType.Int32: returnValue = returnValue + ", SqlDbType.Int"; break;
				case DbType.Int64: returnValue = returnValue + ", SqlDbType.BigInt"; break;
				case DbType.Object: returnValue = returnValue + ", SqlDbType.Variant"; break;
				case DbType.Single: returnValue = returnValue + ", SqlDbType.Real"; break;
				case DbType.String: returnValue = returnValue + ", SqlDbType.NVarChar, " + column.Size.ToString(); break;
				case DbType.StringFixedLength: returnValue = returnValue + ", SqlDbType.NChar, " + column.Size.ToString(); break;
				default:
				{
					return "__UNSUPPORTED__" + column.DataType.ToString();
				}
			}
			
			return returnValue;
		}
		
		public void ExtractProperty(SchemaExplorer.ColumnSchema column)
		{
			Response.WriteLine("");
			GenerateIndent(4);
			Response.Write("if (((SqlParameter)hashParams[\"@" + column.Name + "\"]).Value != DBNull.Value)");
			Response.WriteLine("");
			GenerateIndent(5);
			Response.Write(column.Name + " = (" + GetCSharpVariableType(column.DataType) + ")((SqlParameter)hashParams[\"@" + column.Name + "\"]).Value;");
			Response.WriteLine("");
			GenerateIndent(4);
			Response.Write("else");
			Response.WriteLine("");
			GenerateIndent(5);
			Response.Write(column.Name + " = " + GetCSharpVariableReset(column.DataType) + ";");
			Response.WriteLine("");
		}
		
		public void ExtractProperty(SchemaExplorer.ParameterSchema parameter)
		{
			Response.WriteLine("");
			GenerateIndent(4);
			Response.Write("if (((SqlParameter)hashParams[\"@" + parameter.Name.Remove(0, 1) + "\"]).Value != DBNull.Value)");
			Response.WriteLine("");
			GenerateIndent(5);
			Response.Write(parameter.Name.Remove(0, 1) + " = (" + GetCSharpVariableType(parameter.DataType) + ")((SqlParameter)hashParams[\"@" + parameter.Name.Remove(0, 1) + "\"]).Value;");
			Response.WriteLine("");
			GenerateIndent(4);
			Response.Write("else");
			Response.WriteLine("");
			GenerateIndent(5);
			Response.Write(parameter.Name.Remove(0, 1) + " = " + GetCSharpVariableReset(parameter.DataType) + ";");
			Response.WriteLine("");
		}

		public void ClearProperty(SchemaExplorer.ColumnSchema column, int indentLevel)
		{
			GenerateIndent(indentLevel);
			Response.WriteLine(column.Name + " = " + GetCSharpVariableReset(column.DataType) + ";");
		}
		
		public void ClearProperty(SchemaExplorer.ColumnSchema column)
		{
			GenerateIndent(4);
			Response.WriteLine(column.Name + " = " + GetCSharpVariableReset(column.DataType) + ";");
		}

		public void ClearProperty(SchemaExplorer.ParameterSchema parameter)
		{
			GenerateIndent(4);
			Response.WriteLine(parameter.Name.Remove(0, 1) + " = " + GetCSharpVariableReset(parameter.DataType) + ";");
		}

		public string GetCSharpVariableReset(DbType dbType)
		{
			switch (dbType)
			{
				case DbType.AnsiString: return "String.Empty";
				case DbType.AnsiStringFixedLength: return "String.Empty";
				case DbType.Boolean: return "false";
				case DbType.Binary: return "null";
				case DbType.Byte: return "0";
				case DbType.Currency: return "0";
				case DbType.Date: return "DateTime.MinValue";
				case DbType.DateTime: return "DateTime.MinValue";
				case DbType.Decimal: return "0";
				case DbType.Double: return "0.0";
				case DbType.Guid: return "Guid.Empty";
				case DbType.Int16: return "0";
				case DbType.Int32: return "0";
				case DbType.Int64: return "0";
				case DbType.Object: return "null";
				case DbType.SByte: return "0";
				case DbType.Single: return "0";
				case DbType.String: return "String.Empty";
				case DbType.StringFixedLength: return "String.Empty";
				case DbType.UInt16: return "0";
				case DbType.UInt32: return "0";
				case DbType.UInt64: return "0";
				case DbType.VarNumeric: return "0";
				default:
				{
					return "__UNKNOWN__" + dbType.ToString();
				}
			}
		}
		
		public string GetCSharpVariableType(DbType dbType)
		{
			switch (dbType)
			{
				case DbType.AnsiString: return "string";
				case DbType.AnsiStringFixedLength: return "string";
				case DbType.Binary: return "byte[]";
				case DbType.Boolean: return "bool";
				case DbType.Byte: return "byte";
				case DbType.Currency: return "decimal";
				case DbType.Date: return "DateTime";
				case DbType.DateTime: return "DateTime";
				case DbType.Decimal: return "decimal";
				case DbType.Double: return "double";
				case DbType.Guid: return "Guid";
				case DbType.Int16: return "Int16";
				case DbType.Int32: return "Int32";
				case DbType.Int64: return "Int64";
				case DbType.Object: return "object";
				case DbType.SByte: return "sbyte";
				case DbType.Single: return "float";
				case DbType.String: return "string";
				case DbType.StringFixedLength: return "string";
				case DbType.Time: return "TimeSpan";
				case DbType.UInt16: return "ushort";
				case DbType.UInt32: return "uint";
				case DbType.UInt64: return "ulong";
				case DbType.VarNumeric: return "decimal";
				default:
				{
					return "__UNKNOWN__" + dbType.ToString();
				}
			}
		}
		#endregion
		
		#region Validation Helper Routines
		public bool IsValidDataTable(TableSchema dataTable)
		{
			if ((dataTable != null)) 
			{
				// binding table must have exactly 1 member column in the primary key
				if (dataTable.PrimaryKey.MemberColumns.Count == 1)
				{
					return true;
				}
				else
					ShowValidationError("The source table '" + dataTable.FullName + "' is only allowed 1 member column in the primary key, however " + dataTable.PrimaryKey.MemberColumns.Count.ToString() + " have been found.");
			}
			else
				ShowValidationError("The source table '" + dataTable.FullName + "' is not specified.");

			return false;
		}

		public bool IsValidBindingTable(TableSchema bindingTable)
		{
			if ((bindingTable != null)) 
			{
				// binding table must have exactly 2 member columns in the primary key
				if (bindingTable.PrimaryKey.MemberColumns.Count == 2)
				{
					return true;
				}
				else
					ShowValidationError("The binding table '" + bindingTable.FullName + "' does not contain exactly 2 member columns in the primary key.");
			}
			else
				ShowValidationError("The binding table '" + bindingTable.FullName + "' is not specified.");

			return false;
		}

		public bool IsValidExcludedKey(TableSchema bindingTable, ColumnSchema excludedKey)
		{
			if (excludedKey == null) // OK to have an excluded key value of null
				return true;
			
			if (bindingTable != null) 
			{
				// binding table must have exactly 2 member columns in the primary key
				if (bindingTable.PrimaryKey.MemberColumns.Count == 2)
				{
					// excluded key must be in the binding table
					if ((bindingTable.FullName == excludedKey.Table.FullName))
					{
						// excluded key must be one of the 2 primary key member columns
						if ((bindingTable.PrimaryKey.MemberColumns[0].Name == excludedKey.Name) || 
							(bindingTable.PrimaryKey.MemberColumns[1].Name == excludedKey.Name))
							return true;
						else
							ShowValidationError("The ExcludeKeyField value '" + excludedKey.Name + "' does not exist in either of the primary key member columns for '" + bindingTable.FullName + "'.");
					}
					else
						ShowValidationError("The ExcludeKeyField value '" + excludedKey.Name + "' does not exist in the binding table '" + bindingTable.FullName + "'.");
				}
				else
					ShowValidationError("The binding table '" + bindingTable.FullName + "' does not contain exactly 2 member columns in the primary key.");
			}
			else 
				ShowValidationError("Please specify a binding table for the ExcludeKeyField value '" + excludedKey.Name + "'.");

			return false;
		}

		public bool IsValidStoredProcProperties(CommandSchema storedProc, string name, WRStoredProcReturnType returnType, WRDatabase database)
		{
			if (database != WRDatabase.Ignore)
			{
				if ((name != null) && (name.Trim().Length > 0))
				{
					if (returnType != WRStoredProcReturnType.Ignore)
					{
						if (storedProc.AllOutputParameters.Count > 0)
						{
							// output parameters present, therefore return type must be 'OutputParameter'
							if (returnType == WRStoredProcReturnType.OutputParameters)
							{
								return true;
							}
							else 
								ShowValidationError("There are output parameters in stored procedure '" + storedProc.Name + "', therefore choose 'OutputParameters' and not '" + returnType.ToString() + "' for the StoredProcReturn property.");
						}
						else 
						{
							// output parameters NOT present, therefore must not have return type of 'OutputParameter'
							if (returnType == WRStoredProcReturnType.OutputParameters)
							{
								ShowValidationError("There are no output parameters in stored procedure '" + storedProc.Name + "', therefore do not choose return 'OutputParameters' for the StoredProcReturn property of this stored procedure.");
							}
							else 
								return true;
						}
					}
					else
						ShowValidationError("Please choose a return type for stored procedure '" + storedProc.Name + "' other than 'Ignore'.");
				}
				else
					ShowValidationError("The 'Name' property for stored procedure '" + storedProc.Name + "' is not set.");
			}
			else
				ShowValidationError("Please choose a William Reed database for stored procedure '" + storedProc.Name + "' other than 'Ignore'.");

			return false;
		}
		#endregion
		
		#region SqlCodeTemplate Methods
		/// <summary>
		/// Generates an assignment statement that adds a parameter to a ADO object for the given column.
		/// </summary>
		/// <param name="statementPrefix"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public string GetSqlParameterStatements(string statementPrefix, ColumnSchema column)
		{
			return GetSqlParameterStatements(statementPrefix, column, "sql");
		}
		
		/// <summary>
		/// Generates an assignment statement that adds a parameter to a ADO object for the given column.
		/// </summary>
		/// <param name="statementPrefix"></param>
		/// <param name="column"></param>
		/// <param name="sqlObjectName"></param>
		/// <returns></returns>
		public string GetSqlParameterStatements(string statementPrefix, ColumnSchema column, string sqlObjectName)
		{
			string statements = "\r\n" + statementPrefix + sqlObjectName + ".AddParameter(\"@" + column.Name + "\", SqlDbType." + GetSqlDbType(column) + ", this." + GetPropertyName(column) + GetSqlParameterExtraParams(statementPrefix, column);
	
			return statements.Substring(statementPrefix.Length + 2);
		}
		
		/// <summary>
		/// Returns the C# member variable name for a given identifier.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public string GetMemberVariableName(string value)
		{
			string memberVariableName = "_" + StringUtil.ToCamelCase(value);
			
			return memberVariableName;
		}
		
		/// <summary>
		/// Generates any extra parameters that are needed for the ADO parameter statement.
		/// </summary>
		/// <param name="statementPrefix"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public string GetSqlParameterExtraParams(string statementPrefix, ColumnSchema column)
		{
			switch (column.DataType)
			{
				case DbType.String:
				case DbType.StringFixedLength:
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				{
					if (column.NativeType != "text" && column.NativeType != "ntext" && column.Size != -1)
					{
						return ", " + column.Size + ");";
					}
					else
					{
						return ");";
					}
				}
				case DbType.Decimal:
				{
					return ");\r\n" + statementPrefix + "prm.Scale = " + column.Scale + ";\r\n" + statementPrefix + "prm.Precision = " + column.Precision + ";";
				}
				default:
				{
					return ");";
				}
			}
		}
		
		/// <summary>
		/// Returns a C# member variable declaration statement.
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public string GetMemberVariableDeclarationStatement(ColumnSchema column)
		{
			return GetMemberVariableDeclarationStatement("protected", column);
		}
		
		/// <summary>
		/// Returns a C# member variable declaration statement.
		/// </summary>
		/// <param name="protectionLevel"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public string GetMemberVariableDeclarationStatement(string protectionLevel, ColumnSchema column)
		{
			string statement = protectionLevel + " ";
			statement += GetCSharpVariableType(column) + " " + GetMemberVariableName(column.Name);
			
			string defaultValue = GetMemberVariableDefaultValue(column);
			if (defaultValue != "")
			{
				statement += " = " + defaultValue;
			}
			
			statement += ";";	
			
			return statement;
		}
		
		/// <summary>
		/// Returns a typed C# reader.ReadXXX() statement.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public string GetSqlReaderAssignmentStatement(ColumnSchema column, int index)
		{
			string statement = "if (!reader.IsDBNull(" + index.ToString() + ")) ";
			statement += GetMemberVariableName(column.Name) + " = ";
			
			if (column.Name.EndsWith("TypeCode")) statement += "(" + column.Name + ")";
			
			statement += "reader." + GetReaderMethod(column) + "(" + index.ToString() + ");";
			
			return statement;
		}
		
		/// <summary>
		/// Generates a batch of C# validation statements based on the column.
		/// </summary>
		/// <param name="table"></param>
		/// <param name="statementPrefix"></param>
		/// <returns></returns>
		public string GetValidateStatements(TableSchema table, string statementPrefix)
		{
			string statements = "";
			
			foreach (ColumnSchema column in table.Columns)
			{
				if (IncludeEmptyCheck(column))
				{
					statements += "\r\n" + statementPrefix + "if (" + GetMemberVariableName(column.Name) + " == " + GetMemberVariableDefaultValue(column) + ") this.ValidationErrors.Add(new ValidationError(ValidationTypeCode.Required, \"" + table.Name + "\", \"" + column.Name + "\", \"" + column.Name + " is required.\"));";
				}
				if (IncludeMaxLengthCheck(column))
				{
					statements += "\r\n" + statementPrefix + "if (" + GetMemberVariableName(column.Name) + ".Length > " + column.Size.ToString() + ") this.ValidationErrors.Add(new ValidationError(ValidationTypeCode.MaxLength, \"" + table.Name + "\", \"" + column.Name + "\", \"" + column.Name + " is too long.\"));";
				}
			}
			
			return statements.Substring(statementPrefix.Length + 2);
		}
		
		/// <summary>
		/// Returns the name of the public property for a given column.
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public string GetPropertyName(ColumnSchema column)
		{
			string propertyName = column.Name;
			
			if (propertyName == column.Table.Name + "Name") return "Name";
			if (propertyName == column.Table.Name + "Description") return "Description";
			
			if (propertyName.EndsWith("TypeCode")) propertyName = propertyName.Substring(0, propertyName.Length - 4);
			
			return propertyName;
		}
		
		/// <summary>
		/// Returns the C# variable type based on the given column.
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public string GetCSharpVariableType(ColumnSchema column)
		{
			if (column.Name.EndsWith("TypeCode")) return column.Name;
			
			switch (column.DataType)
			{
				case DbType.AnsiString: return "string";
				case DbType.AnsiStringFixedLength: return "string";
				case DbType.Binary: return "byte[]";
				case DbType.Boolean: return "bool";
				case DbType.Byte: return "byte";
				case DbType.Currency: return "decimal";
				case DbType.Date: return "DateTime";
				case DbType.DateTime: return "DateTime";
				case DbType.Decimal: return "decimal";
				case DbType.Double: return "double";
				case DbType.Guid: return "Guid";
				case DbType.Int16: return "Int16";
				case DbType.Int32: return "Int32";
				case DbType.Int64: return "Int64";
				case DbType.Object: return "object";
				case DbType.SByte: return "sbyte";
				case DbType.Single: return "float";
				case DbType.String: return "string";
				case DbType.StringFixedLength: return "string";
				case DbType.Time: return "TimeSpan";
				case DbType.UInt16: return "ushort";
				case DbType.UInt32: return "uint";
				case DbType.UInt64: return "ulong";
        case DbType.Xml: return "xml";
				case DbType.VarNumeric: return "decimal";
				default:
				{
					return "__UNKNOWN__" + column.NativeType;
				}
			}
		}
		
		/// <summary>
		/// Returns the name of the typed reader method for a given column.
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public string GetReaderMethod(ColumnSchema column)
		{
			switch (column.DataType)
			{
				case DbType.Byte:
				{
					return "GetByte";
				}
				case DbType.Int16:
				{
					return "GetInt16";
				}
				case DbType.Int32:
				{
					return "GetInt32";
				}
				case DbType.Int64:
				{
					return "GetInt64";
				}
				case DbType.AnsiStringFixedLength:
				case DbType.AnsiString:
				case DbType.String:
				case DbType.StringFixedLength:
				{
					return "GetString";
				}
				case DbType.Boolean:
				{
					return "GetBoolean";
				}
				case DbType.Guid:
				{
					return "GetGuid";
				}
				case DbType.Currency:
				case DbType.Decimal:
				{
					return "GetDecimal";
				}
				case DbType.DateTime:
				case DbType.Date:
				{
					return "GetDateTime";
				}
				case DbType.Binary:
				{
					return "GetBytes";
				}
        case DbType.Xml:
        {
          return "GetString";
        }
				default:
				{
					return "__SQL__" + column.DataType;
				}
			}
		}
		
		/// <summary>
		/// Returns the SqlDbType based on a given column.
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public string GetSqlDbType(ColumnSchema column)
		{
			switch (column.NativeType.Trim().ToLower())
			{
				case "bigint": return "BigInt";
				case "binary": return "Binary";
				case "bit": return "Bit";
				case "char": return "Char";
				case "datetime": return "DateTime";
				case "decimal": return "Decimal";
				case "float": return "Float";
				case "image": return "Image";
				case "int": return "Int";
				case "money": return "Money";
				case "nchar": return "NChar";
				case "ntext": return "NText";
				case "numeric": return "Decimal";
				case "nvarchar": return "NVarChar";
				case "real": return "Real";
				case "smalldatetime": return "SmallDateTime";
				case "smallint": return "SmallInt";
				case "smallmoney": return "SmallMoney";
				case "sql_variant": return "Variant";
				case "sysname": return "NChar";
				case "text": return "Text";
				case "timestamp": return "Timestamp";
				case "tinyint": return "TinyInt";
				case "uniqueidentifier": return "UniqueIdentifier";
				case "varbinary": return "VarBinary";
				case "varchar": return "VarChar";
        case "xml": return "Xml";
				default: return "__UNKNOWN__" + column.NativeType;
			}
		}
		
		/// <summary>
		/// Determine if the given column is using a UDT.
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public bool IsUserDefinedType(ColumnSchema column)
		{
			switch (column.NativeType.Trim().ToLower())
			{
				case "bigint":
				case "binary":
				case "bit":
				case "char":
				case "datetime":
				case "decimal":
				case "float":
				case "image":
				case "int":
				case "money":
				case "nchar":
				case "ntext":
				case "numeric":
				case "nvarchar":
				case "real":
				case "smalldatetime":
				case "smallint":
				case "smallmoney":
				case "sql_variant":
				case "sysname":
				case "text":
				case "timestamp":
				case "tinyint":
				case "uniqueidentifier":
				case "varbinary":
        case "xml":
				case "varchar": return false;
				default: return true;
			}
		}
		
		/// <summary>
		/// Returns a default value based on a column's data type.
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public string GetMemberVariableDefaultValue(ColumnSchema column)
		{
			switch (column.DataType)
			{
				case DbType.Guid:
				{
					return "Guid.Empty";
				}
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.String:
				case DbType.StringFixedLength:
				{
					return "String.Empty";
				}
				default:
				{
					return "";
				}
			}
		}
		
		/// <summary>
		/// Determines if the given column's data type requires a max length to be defined.
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public bool IncludeMaxLengthCheck(ColumnSchema column)
		{
			switch (column.DataType)
			{
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.String:
				case DbType.StringFixedLength:
				{
					if (column.NativeType != "text" && column.NativeType != "ntext" && column.Size != -1)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				default:
				{
					return false;
				}
			}
		}
		
		/// <summary>
		/// Determines if a given column should use a check for an Empty value.
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public bool IncludeEmptyCheck(ColumnSchema column)
		{
			if (column.IsPrimaryKeyMember || column.AllowDBNull || column.Name.EndsWith("TypeCode")) return false;
	
			switch (column.DataType)
			{
				case DbType.Guid:
				{
					return true;
				}
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.String:
				case DbType.StringFixedLength:
				{
					return true;
				}
				default:
				{
					return false;
				}
			}
		}
		
		/// <summary>
		/// Returns a T-SQL parameter statement based on the given column.
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public string GetSqlParameterStatement(ColumnSchema column)
		{
			return GetSqlParameterStatement(column, false);
		}
		
		/// <summary>
		/// Returns a camel cased name from the given identifier.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public string GetCamelCaseName(string value)
		{
			return StringUtil.ToCamelCase(value);
		}
		
		/// <summary>
		/// Returns a spaced out version of the identifier.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public string GetSpacedName(string value)
		{
			return StringUtil.ToSpacedWords(value);
		}
		
		/// <summary>
		/// Returns a T-SQL parameter statement based on the given column.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="isOutput"></param>
		/// <returns></returns>
		public string GetSqlParameterStatement(ColumnSchema column, bool isOutput)
		{
			string param = "@" + column.Name + " " + column.NativeType;
			
			if (!this.IsUserDefinedType(column))
			{
				switch (column.DataType)
				{
					case DbType.Decimal:
					{
						param += "(" + column.Precision + ", " + column.Scale + ")";
						break;
					}
					case DbType.AnsiString:
					case DbType.AnsiStringFixedLength:
					case DbType.String:
					case DbType.StringFixedLength:
					{
						if (column.NativeType != "text" && column.NativeType != "ntext")
						{
							if (column.Size > 0)
							{
								param += "(" + column.Size + ")";
							}
              else if (column.Size == -1)
              {
                param += "(max)";
              }
						}
						break;
					}
				}
			}
			
			if (isOutput)
			{
				param += " OUTPUT";
			}
			
			return param;
		}
		#endregion
	}
}
