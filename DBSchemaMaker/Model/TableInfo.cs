namespace DBSchemaMaker.DB.Model
{
    public class TableInfo
    {
        // 테이블의 컬럼 순서 
        public int OridinalPosition { get; set; }
        // 컬럼 이름
        public string ColumnName { get; set; }
        // 해당 컬럼의 데이터 이름 
        public string DataType { get; set; }
        // 데이터 길이 
        public string Length { get; set; }
        // 해당 컬럼에 달린 설명 
        public string Description { get; set; }
        // index 키 잡혀있는지 아닌지
        public string Index { get; set; }
        public TableInfo() { }
    }
}
