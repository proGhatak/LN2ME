using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LN2ME.Models
{
    [Serializable]
    [DataContract]
    public class LN2MERequest<T> : LN2MERequest
    {
    }

    [Serializable]
    [DataContract]
    public class LN2MERequest
    {
    }
}