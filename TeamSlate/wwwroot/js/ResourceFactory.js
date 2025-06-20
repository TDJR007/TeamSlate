// ============================
// RESOURCE FACTORY MODULE
// Creates new resource objects
// ============================

class ResourceFactory {
    static createEmptyResource(weekColumns, designationMap) {
        const defaultDesignationId = designationMap?.$values?.[0]?.id ||
            designationMap?.[0]?.id || 1;

        return {
            Id: null,
            Name: "",
            DesignationId: defaultDesignationId,
            BillableId: 1,
            Availability: "",
            Skills: [],
            WeeklyHours: weekColumns.map(week => ({
                WeekStartDate: week.toISOString(),
                Hours: 0
            }))
        };
    }
}
