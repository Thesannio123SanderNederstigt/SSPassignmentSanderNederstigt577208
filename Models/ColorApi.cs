namespace Models;

 // This is a json object returned from https://www.thecolorapi.com/id
 // This is a cut down version of a lot more info returned from this api endpoint
public struct ColorApi
{
    public ColorName Name { get; set; }
}

public struct ColorName
{
    public string Value { get; set; }
}
