using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Config
{
    public class NullableIntAsIntSerializer : SerializerBase<int?>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, int? value)
        {
            if (value.HasValue)
            {
                context.Writer.WriteInt32(value.Value);
            }
            else {
                context.Writer.WriteNull();
            }
           
        }

        public override int? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            
            var type = context.Reader.GetCurrentBsonType();
            switch (type) {

                case BsonType.Null:
                    context.Reader.ReadNull();
                    return null;
                case BsonType.Int32:
                    return context.Reader.ReadInt32();
                default:
                    throw new NotSupportedException(type + " is not supported");
            }
            
        }

    }
}
