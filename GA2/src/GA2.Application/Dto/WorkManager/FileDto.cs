namespace GA2.Application.Dto
{
    public class FileDto
    {
        private string Base64StringField;

        private string DescriptionField;

        private string DirectoryCodeField;

        private string DocumentTypeCodeField;

        private string ExtField;

        private string GUIDField;
        public FileDto()
        {

        }

        public FileDto(string base64String, string description, string directoryCodeField, string documentTypeCode, string ext, string gUIDField)
        {
            Base64String = base64String;
            Description = description;
            DirectoryCodeField = directoryCodeField;
            DocumentTypeCode = documentTypeCode;
            Ext = ext;
            GUIDField = gUIDField;
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Base64String
        {
            get
            {
                return this.Base64StringField;
            }
            set
            {
                this.Base64StringField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description
        {
            get
            {
                return this.DescriptionField;
            }
            set
            {
                this.DescriptionField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DirectoryCode
        {
            get
            {
                return this.DirectoryCodeField;
            }
            set
            {
                this.DirectoryCodeField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DocumentTypeCode
        {
            get
            {
                return this.DocumentTypeCodeField;
            }
            set
            {
                this.DocumentTypeCodeField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Ext
        {
            get
            {
                return this.ExtField;
            }
            set
            {
                this.ExtField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GUID
        {
            get
            {
                return this.GUIDField;
            }
            set
            {
                this.GUIDField = value;
            }
        }
    }
}