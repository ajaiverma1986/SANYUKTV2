using Newtonsoft.Json;
using SANYUKT.Datamodel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Commonlib.Utility
{
    public static class ExtensionHelper
    {
        /// <summary>
        /// Generic Extension helper function which serializes the specified object 
        /// </summary>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        public static string Serialize(this object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }

        /// <summary>
        /// Generic Extension helper function which deserializes the specified object 
        /// into the specified class type object
        /// </summary>
        /// <typeparam name="T1">Source Object type</typeparam>
        /// <typeparam name="T2">Required result object type</typeparam>
        /// <param name="objectToSerialize">object to be serilized</param>
        /// <returns></returns>
        public static T2 Deserialize<T1, T2>(this T1 objectToSerialize)
        {
            return JsonConvert.DeserializeObject<T2>(JsonConvert.SerializeObject(objectToSerialize));
        }

        /// <summary>
        /// Generic Extension helper function which deserializes the object 
        /// into the specified class type object
        /// </summary>
        /// <typeparam name="T">Required result object type</typeparam>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this object objectToSerialize)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(objectToSerialize));
        }

        /// <summary>
        /// Generic Extension helper function which deserializes the object 
        /// into the specified class type object
        /// </summary>
        /// <typeparam name="T">Required result object type</typeparam>
        /// <param name="objectToDeserialize"></param>
        /// <returns></returns>
        public static T DeserializeListResponse<T>(this object objectToDeserialize)
        {
            ListResponse response = objectToDeserialize.Deserialize<ListResponse>();
            return response.Result.Deserialize<T>();
        }

        /// <summary>
        /// Generic Extension helper function which deserializes the object 
        /// into the specified class type object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToDeserialize"></param>
        /// <returns></returns>
        public static T DeserializeSimpleResponse<T>(this object objectToDeserialize)
        {
            SimpleResponse response = objectToDeserialize.Deserialize<SimpleResponse>();
            return response.Result.Deserialize<T>();
        }
    }
}
