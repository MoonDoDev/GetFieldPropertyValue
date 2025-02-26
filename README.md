# GetFieldPropertyValue

Este es un proyecto demo construido en Visual Studio 2022, y en C# de .NET 8, con el propósito de mostrar una manera cíclica de obtener los valores de los campos o propiedades de una clase/record.

Para obtener los valores de los campos/propiedades, es necesario suministrar sus nombres, los cuales estamos almacenando en un array de tipo string `string[]`, e igualmente en una lista de strings `List<string>`.
Estamos utilizando dos tipos diferentes para almacenar los nombres de los campos/propiedades para darle un valor adicional a este demo, y es mostrar el **Benchmark** en el uso de una sentencia `for` con un arreglo, y una sentencia `foreach` con una colección `IEnumerable`.

En el presente proyecto tenemos una clase llamada `RecordData`, la cual tiene varios campos y algunas propiedades, y para obtener sus valores, hemos creado dos métodos, los cuales son: 

```
internal T GetFieldValue<T>( string fieldName ) 
internal T GetPropertyValue<T>( string propertyName )
```

La primera, como su nombre lo supone, es para obtener los valores de los campos, y la segunda para obtener los valores de las propiedades de la clase.

Adicionalmente encontraremos la clase llamada `BenchmarkClass`, con la cual estamos evaluando el rendimiento al momento de obtener los valores de los campos/propiedades de la clase, usando `for` y `foreach`. El resultado de dicha evaluación se muestra a continuación (saca tus propias conclusiones):

```
|---------------------------------------------------------|
| Method                 | Mean     | Error    | StdDev   |
|----------------------- |---------:|---------:|---------:|
| GetFieldsByFor         | 778.3 ns | 14.65 ns | 14.39 ns |
| GetPropertiesByFor     | 259.2 ns |  5.30 ns |  7.26 ns |
| GetFieldsByForeach     | 798.3 ns | 28.84 ns | 85.03 ns |
| GetPropertiesByForeach | 278.5 ns | 12.58 ns | 36.69 ns |
|---------------------------------------------------------|
| * Legends *                                             |
| Mean   : Arithmetic mean of all measurements            |
| Error  : Half of 99.9% confidence interval              |
| StdDev : Standard deviation of all measurements         |
| 1 ns   : 1 Nanosecond(0.000000001 sec)                  |
|---------------------------------------------------------|

```
> [!IMPORTANT]
> 
> Las métricas aqui expuestas pueden variar, dependiendo de las características del equipo donde se ejecute el test.

## Dependencias

```
"BenchmarkDotNet" Version="0.14.0"
```

---------

[**YouTube**](https://www.youtube.com/@hectorgomez-backend-dev/featured) - 
[**LinkedIn**](https://www.linkedin.com/in/hectorgomez-backend-dev/) - 
[**GitHub**](https://github.com/MoonDoDev/Jubatus.WebApi.Extensions)
