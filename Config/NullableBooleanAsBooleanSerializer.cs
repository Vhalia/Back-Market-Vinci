using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Config
{
    public class NullableBooleanAsBooleanSerializer : SerializerBase<Boolean?>
    {

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Boolean? value)
        {
            if (value.HasValue)
            {
                context.Writer.WriteBoolean(value.Value);
            }
            else
            {
                context.Writer.WriteNull();
            }

        }

        public override Boolean? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            Console.WriteLine("boolean converter");
            var type = context.Reader.GetCurrentBsonType();
            switch (type)
            {

                case BsonType.Null:
                    return null;
                case BsonType.Boolean:
                    return context.Reader.ReadBoolean();
                default:
                    throw new NotSupportedException(type + " is not supported");
            }

        }

    }
}
