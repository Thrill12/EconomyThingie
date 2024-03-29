﻿using Newtonsoft.Json;
using RequestLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class Response
    {

        private readonly object data;
        public readonly Type type;

        public bool HasData => data != null;
        public string DataString => $"{type.AssemblyQualifiedName}\n{JsonConvert.SerializeObject(data, SerializationSettings.current)}";

        public Response(object data, Type type)
        {
            this.data = data;
            this.type = type;
        }

        public static Response Ok()
        {
            return new Response(null, null);
        }

        public static Response From<T>(T data)
        {
            return new Response(data, data.GetType());
        }

    }
}
