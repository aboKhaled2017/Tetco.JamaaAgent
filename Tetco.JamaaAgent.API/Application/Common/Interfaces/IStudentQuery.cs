﻿using Application.Agent.Queries.GetDynamicQueryData;
using Application.Agent.Queries.Students.GetPage;
using Application.Agent.Queries.Students.GetStudentMetaData;

namespace Application.Common.Interfaces
{
    public interface IStudentQuery
    {
        Task<IEnumerable<ViewDetail>> GetAllAsync(int pageSize, int pageNumber, string schemaName, string masterViewName, List<string> relatedViews, string associationColumnName, string columnNameFilter, string from, string to);
        Task<IEnumerable<ViewsMetaData>> GetColumnInformation(string schemaName,List<string> views);
    }
}
