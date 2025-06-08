public class Resource : StaticUnitBase
{
    public override bool IsInitialized => true;

    // здесь вместо public set можно использовать метод Reserve(),
    // который возвращает ResourceReservation с методом ReleaseResource
    // для того, чтобы освободить ресурс мог только тот, кто его зарезервировал
    public bool IsReserved { get; set; } = false;

    public void Collect()
    {
        DestroyImmediate(gameObject);
    }
}