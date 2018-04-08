public interface IAffectedByTime
{
    object GetData();
    void SetData(object data);
    bool Enabled { get; set; }
}
