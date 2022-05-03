using Kendo.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using HIsabKaro.Controllers.Filters;
using HIsabKaro.Cores.Helpers;

namespace TeamIN.ReckonIN.API.Helpers.Kendo
{
    public class KendoDataSourceRequestModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            string Sort = "";
            string Group = "";
            string Aggregates = "";
            string Filter = "";
            int PageNumber = 0;
            int PageSize = 0;
            var dataSourceRequest = new DataSourceRequest();
            try
            {
                if (this.TryGetValue<string>(bindingContext, DataSourceRequestUrlParameters.Sort, ref Sort))
                {
                    dataSourceRequest.Sorts = DataSourceDescriptorSerializer.Deserialize<SortDescriptor>(Sort);
                }
            }
            catch (Exception)
            {
                throw new HttpResponseException() { Status = 400, Value = "Invalid sort parameter!" };
            }

            try
            {
                if (this.TryGetValue<int>(bindingContext, DataSourceRequestUrlParameters.Page, ref PageNumber))
                {
                    dataSourceRequest.Page = PageNumber;
                }
            }
            catch (Exception)
            {
                throw new HttpResponseException() { Status = 400, Value = "Invalid Page Number parameter!" };
            }

            try
            {
                if (this.TryGetValue<int>(bindingContext, DataSourceRequestUrlParameters.PageSize, ref PageSize))
                {
                    dataSourceRequest.PageSize = PageSize;
                }
            }
            catch (Exception)
            {
                throw new HttpResponseException() { Status = 400, Value = "Invalid Page Size parameter!" };
            }

            try
            {
                if (this.TryGetValue<string>(bindingContext, DataSourceRequestUrlParameters.Filter, ref Filter))
                {
                    dataSourceRequest.Filters = FilterDescriptorFactory.Create(Filter);
                }
            }
            catch (Exception)
            {
                throw new HttpResponseException() { Status = 400, Value = "Invalid Filter parameter!" };
            }

            try
            {
                if (this.TryGetValue<string>(bindingContext, DataSourceRequestUrlParameters.Group, ref Group))
                {
                    dataSourceRequest.Groups = DataSourceDescriptorSerializer.Deserialize<GroupDescriptor>(Group);
                }
            }
            catch (Exception)
            {
                throw new HttpResponseException() { Status = 400, Value = "Invalid Group parameter!" };
            }

            try
            {
                if (this.TryGetValue<string>(bindingContext, DataSourceRequestUrlParameters.Aggregates, ref Aggregates))
                {
                    dataSourceRequest.Aggregates = DataSourceDescriptorSerializer.Deserialize<AggregateDescriptor>(Aggregates);
                }
            }
            catch (Exception)
            {
                throw new HttpResponseException() { Status = 400, Value = "Invalid Aggregate parameter!" };
            }

            bindingContext.Result =ModelBindingResult.Success(dataSourceRequest);

            return Task.CompletedTask;
        }

        private bool TryGetValue<T>(ModelBindingContext bindingContext, string key, ref T result)
        {
            var value = bindingContext.ValueProvider.GetValue(key);
            if (value == ValueProviderResult.None)
            {
                result = default;
                return false;
            }
            result = (T)Convert.ChangeType(value, typeof(T));
            return true;
        }

    }

    public class KendoFunctions {
        public static List<Columns> getKendoColumns(DataColumnCollection cols)
        {
            var c = new List<Columns>();
            foreach (DataColumn col in cols)
            {
                string ChangedDataType = "";
                if (col.DataType.Name == "String")
                {
                    ChangedDataType = "string";
                }
                else if (col.DataType.Name == "DateTime")
                {
                    ChangedDataType = "date";
                }
                else if (col.DataType.Name == "Boolean")
                {
                    ChangedDataType = "boolean";
                }
                else if (col.DataType.Name == "Guid")
                {
                    ChangedDataType = "string";
                }
                else if (col.DataType.Name == "Decimal" | col.DataType.Name == "Single" | col.DataType.Name == "Double")
                {
                    ChangedDataType = "decimal";
                }
                else
                {
                    ChangedDataType = "number";
                }

                var newCol = new Columns();
                newCol.ColumnName = col.ColumnName;
                newCol.ColumnNameFormatted = col.ColumnName;
                newCol.ColumnType = ChangedDataType;
                c.Add(newCol);
            }

            return c;
        }

        public partial class Columns
        {
            public Columns()
            {
                _ColumnName = "";
                _ColumnNameFormatted = "";
                _ColumnType = "";
            }

            private string _ColumnName;

            public string ColumnName
            {
                get
                {
                    return _ColumnName.Replace(" ", "_");
                }

                set
                {
                    _ColumnName = value;
                }
            }

            private string _ColumnNameFormatted;

            public string ColumnNameFormatted
            {
                get
                {
                    return _ColumnNameFormatted.AddSpacesToSentence();
                }

                set
                {
                    _ColumnNameFormatted = value;
                }
            }

            private string _ColumnType;

            public string ColumnType
            {
                get
                {
                    return _ColumnType;
                }

                set
                {
                    _ColumnType = value;
                }
            }

            private string _Parent;

            public string Parent
            {
                get
                {
                    return _Parent;
                }

                set
                {
                    _Parent = value;
                }
            }

            private string _GrandParent;

            public string GrandParent
            {
                get
                {
                    return _GrandParent;
                }

                set
                {
                    _GrandParent = value;
                }
            }
        }

        public static string getKendoConditionToSqlCondition(FilterOperator Op, int index)
        {
            var sqlOperators = new[] { " < @Value ", " <= @Value ", " = @Value ", " <> @Value ", " >= @Value ", " > @Value ", " like @Value ", " like @Value ", " like @Value ", " like @Value ", " not like @Value ", " is null ", " is not null ", " = '' ", " <> '' " };
            string r = sqlOperators[(int)Op].Replace("@Value", "@Value" + index.ToString());
            return r;
        }

        public static string getKendoCondition(FilterOperator Op)
        {
            var sqlOperators = new[] { " <", " <= ", " = ", " <> ", " >= ", " > ", " like  ", " like  ", " like  ", " like  ", " not like  ", " is null ", " is not null ", " '' ", " <> '' " };
            var r = sqlOperators[(int)Op].ToString();
            return r;
        }

        public List<FilterDescriptor> GetAllFilterDescriptors(DataSourceRequest request)
        {
            var allFilterDescriptors = new List<FilterDescriptor>();
            RecurseFilterDescriptors(request.Filters, allFilterDescriptors);
            return allFilterDescriptors;
        }

        private void RecurseFilterDescriptors(IList<IFilterDescriptor> requestFilters, List<FilterDescriptor> allFilterDescriptors)
        {
            foreach (var filterDescriptor in requestFilters)
            {
                if (filterDescriptor is FilterDescriptor)
                {
                    allFilterDescriptors.Add((FilterDescriptor)filterDescriptor);
                }
                else if (filterDescriptor is CompositeFilterDescriptor)
                {
                    RecurseFilterDescriptors(((CompositeFilterDescriptor)filterDescriptor).FilterDescriptors, allFilterDescriptors);
                }
            }
        }

        private int lev = 0;

        public List<CustomCompositeFilterDescriptor> GetAllCompositeFilterDescriptors(DataSourceRequest request)
        {
            var allCompositeFilterDescriptors = new List<CustomCompositeFilterDescriptor>();
            RecurseCompositeFilterDescriptors(request.Filters, allCompositeFilterDescriptors, default);
            return allCompositeFilterDescriptors;
        }

        private void RecurseCompositeFilterDescriptors(IList<IFilterDescriptor> requestFilters, List<CustomCompositeFilterDescriptor> allCompositeFilterDescriptors, int? ParentLevel)
        {
            lev += 1;
            foreach (var filterDescriptor in requestFilters)
            {
                if (filterDescriptor is CompositeFilterDescriptor)
                {
                    var d = new CustomCompositeFilterDescriptor(filterDescriptor as CompositeFilterDescriptor);
                    d.Level = (int)lev;
                    d.ParentLevel = ParentLevel;
                    allCompositeFilterDescriptors.Add(d);
                    RecurseCompositeFilterDescriptors(((CompositeFilterDescriptor)filterDescriptor).FilterDescriptors, allCompositeFilterDescriptors, d.Level);
                }
            }
        }

        public class CustomCompositeFilterDescriptor : CompositeFilterDescriptor
        {
            public CustomCompositeFilterDescriptor(CompositeFilterDescriptor Parent)
            {
                this.FilterDescriptors = Parent.FilterDescriptors;
                this.LogicalOperator = Parent.LogicalOperator;
                _Clause = "";
                _Level = 0;
                _ParentLevel = default;
            }

            private string _Clause;

            public string Clause
            {
                get
                {
                    return _Clause;
                }

                set
                {
                    _Clause = value;
                }
            }

            private int _Level;

            public int Level
            {
                get
                {
                    return _Level;
                }

                set
                {
                    _Level = value;
                }
            }

            private int? _ParentLevel;

            public int? ParentLevel
            {
                get
                {
                    return _ParentLevel;
                }

                set
                {
                    _ParentLevel = value;
                }
            }
        }

        public partial class Sql
        {
            public partial class Where
            {
                // Sub New()
                // _Clause = ""
                // _SqlParameters = New List(Of SqlClient.SqlParameter)
                // End Sub
                private string _Clause;

                public string Clause
                {
                    get
                    {
                        return _Clause;
                    }

                    set
                    {
                        _Clause = value;
                    }
                }

                private List<System.Data.SqlClient.SqlParameter> _SqlParameters;

                public List<System.Data.SqlClient.SqlParameter> SqlParameters
                {
                    get
                    {
                        return _SqlParameters;
                    }

                    set
                    {
                        _SqlParameters = value;
                    }
                }
            }
        }
    }

    internal static partial class ExtensionMethods
    {
        public static SqlParameter AddWithNullableValue(this SqlParameterCollection collection, string parameterName, object value)
        {
            if (value == null)
            {
                return collection.AddWithValue(parameterName, DBNull.Value);
            }
            else if (value.ToString() == "")
            {
                return collection.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                return collection.AddWithValue(parameterName, value);
            }
        }

        public static IEnumerable<IEnumerable<int>> Combinations(this IEnumerable<int> elements, int k)
        {
            return k == 0 ? (new[] { new int[0] }) : elements.SelectMany((e, i) => elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }

        public static KendoFunctions.Sql.Where WhereClause(this DataSourceRequest Request)
        {
            KendoFunctions.Sql.Where WhereClauseRet = default;
            WhereClauseRet = new KendoFunctions.Sql.Where();
            WhereClauseRet.SqlParameters = new List<SqlParameter>();
            if (Request.Filters is object)
            {
                int i = 0;
                void setClause(KendoFunctions.CustomCompositeFilterDescriptor cfd)
                {
                    int fcount = 0;
                    foreach (var fd in cfd.FilterDescriptors)
                    {
                        if (fd is FilterDescriptor)
                        {
                            fcount = fcount + 1;
                            FilterDescriptor f = (FilterDescriptor)fd;
                            var Condition = KendoFunctions.getKendoConditionToSqlCondition(f.Operator, i);
                            cfd.Clause = cfd.Clause + "[" + f.Member + "]" + Condition + " " + cfd.LogicalOperator.ToString() + " ";

                            var op = (int)f.Operator;

                            if (Condition.Contains("@Value"))
                            {
                                if (Condition.Contains("like "))
                                {
                                    if (op == 6)
                                    {
                                        f.Value = f.Value + "%";
                                    }
                                    else if (op == 7)
                                    {
                                        f.Value = "%" + f.Value;
                                    }
                                    else if (op == 8 || op == 9 || op == 10)
                                    {
                                        f.Value = "%" + f.Value + "%";
                                    }
                                }
                                //else if (Condition.Contains("is "))
                                //{
                                //    if (op == 11 && op == 12)
                                //    {
                                //        f.Value = null;
                                //    }
                                //}

                                var Param = new SqlParameter("Value" + i.ToString(), f.Value);
                                Param.SourceColumn = f.Member;
                                WhereClauseRet.SqlParameters.Add(Param);
                                i += 1;
                            }
                            else
                            {
                                WhereClauseRet.SqlParameters.Add(new SqlParameter(f.Member, DBNull.Value) { SourceColumn = f.Member });
                                i += 1;
                            }
                        }
                    }

                    if (fcount > 0)
                    {
                        cfd.Clause = cfd.Clause.Substring(0, cfd.Clause.Length - (cfd.LogicalOperator.ToString().Length + 2));
                        cfd.Clause = "(" + cfd.Clause + ")";
                    }
                };
                var CFDs = new KendoFunctions().GetAllCompositeFilterDescriptors(Request);
                CFDs.Reverse();
                foreach (var cfd in CFDs)
                {
                    var ChildCFDs = from x in CFDs
                                    where x.ParentLevel.Equals(cfd.Level)
                                    select x;
                    foreach (var childcfd in ChildCFDs)
                        cfd.Clause = cfd.Clause + childcfd.Clause + " " + cfd.LogicalOperator.ToString() + " ";
                    setClause(cfd);
                    if (ChildCFDs.Count() > 1)
                    {
                        cfd.Clause = cfd.Clause.Substring(0, cfd.Clause.Length - (cfd.LogicalOperator.ToString().Length + 2));
                        cfd.Clause = "(" + cfd.Clause.Trim() + ")";
                    }
                }

                if (CFDs.Count == 0)
                {
                    if (Request.Filters.Count == 1)
                    {
                        FilterDescriptor f = Request.Filters.First() as FilterDescriptor;
                        var cfd = new KendoFunctions.CustomCompositeFilterDescriptor(new CompositeFilterDescriptor());
                        var Condition = KendoFunctions.getKendoConditionToSqlCondition(f.Operator, i);
                        cfd.Clause = cfd.Clause + "[" + f.Member + "] " + Condition;
                        CFDs.Add(cfd);
                        var op = (int)f.Operator;
                        if (Condition.Contains("@Value"))
                        {
                            if (Condition.Contains("like "))
                            {
                                if (op == 6)
                                {
                                    f.Value = f.Value + "%";
                                }
                                else if (op == 7)
                                {
                                    f.Value = "%" + f.Value;
                                }
                                else if (op == 8 || op == 9 || op == 10)
                                {
                                    f.Value = "%" + f.Value + "%";
                                }
                            }
                            //else if (Condition.Contains("is "))
                            //{
                            //    if (op == 11 && op == 12)
                            //    {
                            //        f.Value = null;
                            //    }
                            //}

                            var Param = new SqlParameter("Value" + i.ToString(), f.Value);
                            Param.SourceColumn = f.Member;
                            WhereClauseRet.SqlParameters.Add(Param);
                            i = i + 1;
                        }
                        else
                        {
                            WhereClauseRet.SqlParameters.Add(new SqlParameter(f.Member, DBNull.Value) { SourceColumn = f.Member });
                            i += 1;
                        }
                    }
                }

                if (CFDs.Count != 0)
                {
                    WhereClauseRet.Clause = CFDs.Last().Clause;
                }
                else
                {
                    WhereClauseRet.Clause = "";
                }

                WhereClauseRet.Clause = System.Text.RegularExpressions.Regex.Replace(WhereClauseRet.Clause, @"\s+", " ");
            }
            else
            {
                WhereClauseRet.Clause = "";
            }

            return WhereClauseRet;
        }

        public static string SortClause(this DataSourceRequest Request)
        {
            if (Request.Sorts != null)
            {
                var oQ = (from x in Request.Sorts
                          where x.Member != null
                          select ("[" + x.Member.Replace("_", "") + "] " + (x.SortDirection == ListSortDirection.Ascending ? "asc" : "desc"))).ToArray();
                return String.Join(",", oQ);
            }
            else
            {
                return "";
            }
        }
    }


}
