namespace GA2.Application.Dto.Reportes
{
    public class ColumnHeaderInfo
    {
        public string Name { get; set; }
        public ColumnFormatType? FormatType { get; set; }

        public ColumnHeaderInfo(string name, ColumnFormatType? formatType = null)
        {
            Name = name;
            FormatType = formatType;
        }


        public static implicit operator ColumnHeaderInfo(string name)
        {
            return new ColumnHeaderInfo(name);
        }

        public static implicit operator ColumnHeaderInfo((string name, ColumnFormatType? formatType) header)
        {
            return new ColumnHeaderInfo(header.name, header.formatType);
        }
    }
}
