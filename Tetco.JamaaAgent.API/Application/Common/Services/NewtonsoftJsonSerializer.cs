using Domain.Common.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace Application.Common.Services
{
    public class NewtonsoftJsonSerializer : ISerializer
      {
      private int _bufferSize;
      private Encoding _encoding;
      public NewtonsoftJsonSerializer ( )
         {
         _bufferSize = 4 * 1024;
         _encoding = Encoding.UTF8;
         }
      public async Task<string> SerializeAsync<T> ( T data )
         {
         string result = string.Empty;

         using ( var stream = await SerializeToMemoryStreamAsync ( data ) )
            {
            using ( var reader = new StreamReader ( stream , _encoding , false , _bufferSize , true ) )
               {
               result = await reader.ReadToEndAsync ( );
               }
            }

         return result;
         }
      public async Task<MemoryStream> SerializeToMemoryStreamAsync<T> ( T data )
         {
         return await Task.Run ( ( ) =>
         {
            return SerializeToMemoryStream ( data );
         } );
         }
      public async Task<T> DeserializeAsync<T> ( string jsonData )
         {
         return await Task.Run ( ( ) =>
         {
            return Deserialize<T> ( new MemoryStream ( Encoding.UTF8.GetBytes ( jsonData ) ) );
         } );
         }
      public async Task<T> DeserializeAsync<T> ( MemoryStream jsonStream )
         {
         return await Task.Run ( ( ) =>
         {
            return Deserialize<T> ( jsonStream );
         } );
         }
      public string Serialize<T> ( T data )
         {
         string result = string.Empty;

         using ( var stream = SerializeToMemoryStream ( data ) )
            {
            using ( var reader = new StreamReader ( stream , _encoding , false , _bufferSize , true ) )
               {
               result = reader.ReadToEnd ( );
               }
            }

         return result;
         }
      public T Deserialize<T> ( string jsonData )
         {
         if ( typeof ( T ) == typeof ( string ) )
            {
            return ( T ) Convert.ChangeType ( jsonData , typeof ( T ) );
            }
         else
            {
            return Deserialize<T> ( new MemoryStream ( Encoding.UTF8.GetBytes ( jsonData ) ) );
            }
         }
      public T Deserialize<T> ( MemoryStream jsonStream )
         {
         var result = default ( T );

         jsonStream.Seek ( 0 , SeekOrigin.Begin );

         if ( typeof ( T ) == typeof ( string ) )
            {
            using ( var streamReader = new StreamReader ( jsonStream , _encoding , false , _bufferSize , true ) )
               {
               var jsonData = streamReader.ReadToEnd ( );

               return ( T ) Convert.ChangeType ( jsonData , typeof ( T ) );
               }
            }
         else
            {
            using ( var streamReader = new StreamReader ( jsonStream , _encoding , false , _bufferSize , true ) )
               {
               using ( var jsonReader = new JsonTextReader ( streamReader ) )
                  {
                  var deserializer = new JsonSerializer ( );

                  result = deserializer.Deserialize<T> ( jsonReader );
                  }
               }
            }

         return result;
         }
      public dynamic Deserialize ( string jsonData )
         {
         return JsonConvert.DeserializeObject ( jsonData );
         }
      public MemoryStream SerializeToMemoryStream<T> ( T data )
         {
         var stream = new MemoryStream ( );

         using ( var streamWriter = new StreamWriter ( stream , _encoding , _bufferSize , true ) )
            {
            using ( var jsonWriter = new JsonTextWriter ( streamWriter ) )
               {
               var serializer = new JsonSerializer ( );

               serializer.Serialize ( jsonWriter , data );
               }
            }

         stream.Seek ( 0 , SeekOrigin.Begin );

         return stream;
         }
      }
   }
