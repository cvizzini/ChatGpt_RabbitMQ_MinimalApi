using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalApi.Core.Model;

public class DallEInput
{

    public string Prompt { get; set; }
    public short? N { get; set; }
    public string Size { get; set; }
}

// model for the image url
public class Link
{
    public string Url { get; set; }
}

// model for the DALL E api response
public class ResponseModel
{
    public long Created { get; set; }
    public List<Link> Data { get; set; }
}