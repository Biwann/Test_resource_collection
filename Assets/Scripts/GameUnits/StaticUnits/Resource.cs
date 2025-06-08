public class Resource : StaticUnitBase
{
    public override bool IsInitialized => true;

    // ����� ������ public set ����� ������������ ����� Reserve(),
    // ������� ���������� ResourceReservation � ������� ReleaseResource
    // ��� ����, ����� ���������� ������ ��� ������ ���, ��� ��� ��������������
    public bool IsReserved { get; set; } = false;

    public void Collect()
    {
        DestroyImmediate(gameObject);
    }
}