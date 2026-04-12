using System;
using System.Collections.Generic;
using System.Text;

namespace ZaminX.BuildingBlocks.Data.Abstractions.Models;

public class PagedQuery
{
    private int _pageNumber = 1;
    private int _pageSize = 20;

    public int PageNumber
    {
        get => _pageNumber;
        init => _pageNumber = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value < 1 ? 20 : value;
    }

    public bool IncludeTotalCount { get; init; } = true;

}