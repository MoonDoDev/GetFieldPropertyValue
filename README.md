# Lectura cíclica de campos y propiedades de una clase/record - GetFieldPropertyValue

Este es un proyecto demo construido en Visual Studio 2022, y en C# 12 de .NET 8, con el propósito de mostrar una alternativa cíclica para obtener los valores de los campos/**fields** o propiedades/**properties** de una clase/record.

Para nuestro propósito usaremos 3 herramientas que nos brinda C#:
- [x]  **`Dictionary`**: La cual es una clase que nos provee una forma eficiente de almacenar y obtener datos en pares de clave/valor (**key-value pairs**).
- [x]  **`Generics`**: Nos permite diseñar clases y métodos que posponen la especificación del tipo de dato de uno o más parámetros hasta que utilice la clase o el método en su código.
- [x]  **`Reflection`**: La reflexión en C# es una característica que nos permite examinar y modificar los metadatos y objetos de un programa durante el tiempo de ejecución.

A continuación vamos a analizar tres formas diferentes e igualmente válidas, para cumplir nuestro propósito, pero que si nos enfocamos en encontrar la forma más eficiente, los resultamos del **Benchmark** nos lo dirá.

Primero vamos a crear una clase llamada `RecordData`, la cual tiene varios campos y propiedades, y para obtener sus valores, hemos creado en ella dos métodos, los cuales están implementados usando `Generics`: 
```
public T GetFieldValue<T>( string fieldName ) 
public T GetPropertyValue<T>( string propertyName )
```
El primero, como su nombre lo supone, es para obtener los valores de los campos, y el segundo para obtener los valores de las propiedades de la clase.

Para obtener los valores de los campos y/o propiedades de la clase, usaremos `Reflection`, suministrando sus nombres y el tipo de dato que almacena, apoyándonos para ésto, de un objeto de la clase `Dictionary<string, Type>`, donde la llave `TKey` es el nombre del campo/propiedad y el valor `TValue` es el tipo de dato que almacena.

Con el propósito de analizar las diferentes formas, hemos creado la clase `BenchmarkClass`, en donde hemos definido  métodos para trabajar con los campos y otros para las propiedades:
- [x]  En los métodos con nombre terminados en **ByOnlyReflection** estamos usando 100% `Reflection`.
- [x]  En los métodos con nombre terminados en **ByGenericAndReflection** estamos usando `Reflection` y `Generics` para invocar el método *GetFieldValue* o *GetPropertyValue*, según corresponda.
- [x]  Y finalmente, en los métodos terminados en **ByGenericReflectionAndDictionary** estamos usando `Reflection`,`Generics` y `Dictionary` para suministrar los nombres de los campos/propiedades y sus correspondientes tipos de dato.

El resultado de dicha evaluación se muestra a continuación (saca tus propias conclusiones):
```
|-----------------------------------------------------------------------------------------|
| Method                                    | Mean     | Error     | StdDev    | Median   |
|------------------------------------------ |---------:|----------:|----------:|---------:|
| GetFieldsByOnlyReflection                 | 1.442 us | 0.0182 us | 0.0170 us | 1.444 us |
| GetPropsByOnlyReflection                  | 1.175 us | 0.0156 us | 0.0138 us | 1.173 us |
|------------------------------------------ |---------:|----------:|----------:|---------:|
| GetFieldsByGenericAndReflection           | 4.366 us | 0.0561 us | 0.0525 us | 4.371 us |
| GetPropsByGenericAndReflection            | 4.701 us | 0.2165 us | 0.6314 us | 4.537 us |
| GetFieldsByGenericReflectionAndDictionary | 4.587 us | 0.2291 us | 0.6684 us | 4.319 us |
| GetPropsByGenericReflectionAndDictionary  | 4.206 us | 0.1623 us | 0.4579 us | 4.187 us |
|-----------------------------------------------------------------------------------------|
| * Legends *                                                                             |
| Mean   : Arithmetic mean of all measurements                                            |
| Error  : Half of 99.9% confidence interval                                              |
| StdDev : Standard deviation of all measurements                                         |
| Median : Value separating the higher half of all measurements (50th percentile)         |
| 1 us   : 1 Microsecond (0.000001 sec)                                                   |
|-----------------------------------------------------------------------------------------|
```
> [!IMPORTANT]
> 
> Las métricas aqui expuestas pueden variar, dependiendo de las características del equipo donde se ejecute el test.

## Conclusiones
Podemos concluir que para nuestro ejemplo y propósito, nos funciona mucho mejor si usamos 100% `Reflection`, y adicionalmente podemos ver cierta diferencia entre la lectura de los valores de los campos y las propiedades, que para nuestro caso son 5 y 5.

## Dependencias
```
"BenchmarkDotNet" Version="0.14.0"
```
---------
[**YouTube**](https://www.youtube.com/@hectorgomez-backend-dev/featured) - 
[**LinkedIn**](https://www.linkedin.com/in/hectorgomez-backend-dev/) - 
[**GitHub**](https://github.com/MoonDoDev/Jubatus.WebApi.Extensions)
