namespace Domain.Common.Interfaces
   {
   public interface ISerializer
      {
      Task<string> SerializeAsync<T> ( T data );

      Task<MemoryStream> SerializeToMemoryStreamAsync<T> ( T data );

      Task<T> DeserializeAsync<T> ( string jsonData );

      Task<T> DeserializeAsync<T> ( MemoryStream jsonStream );

      string Serialize<T> ( T data );

      MemoryStream SerializeToMemoryStream<T> ( T data );

      T Deserialize<T> ( string jsonData );

      T Deserialize<T> ( MemoryStream jsonStream );

      dynamic Deserialize ( string jsonData );
      }
   }
