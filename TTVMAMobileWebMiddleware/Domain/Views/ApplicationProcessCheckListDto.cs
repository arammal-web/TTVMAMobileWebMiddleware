namespace TTVMAMobileWebMiddleware.Domain.Views
{
    public sealed class ApplicationProcessCheckListDto
    {
        public int ChekListId { get; set; }
        public bool IsDocRequired { get; set; }

        // CheckList multilingual (item)
        public string? CheckListEn { get; set; }
        public string? CheckListAr { get; set; }
        public string? CheckListFr { get; set; }

        // CheckListGroup multilingual (group)
        public string? CheckListGroupEn { get; set; }
        public string? CheckListGroupAr { get; set; }
        public string? CheckListGroupFr { get; set; }
    }
}
