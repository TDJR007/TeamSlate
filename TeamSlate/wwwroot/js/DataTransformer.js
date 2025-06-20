// ============================
// DATA TRANSFORMER MODULE
// Handles data normalization and transformation
// ============================

class DataTransformer {
    static normalizeResourceData(data) {
        const resources = data?.$values || data || [];
        return resources.map(resource => ({
            Id: resource.id,
            Name: resource.name,
            DesignationId: resource.designationId,
            BillableId: resource.billableId,
            Availability: resource.availability,
            Skills: this.normalizeSkills(resource.skills),
            WeeklyHours: this.normalizeWeeklyHours(resource.weeklyHours)
        }));
    }

    static normalizeSkills(skills) {
        const skillsArray = skills?.$values || skills || [];
        return skillsArray.map(skill => ({
            SkillId: skill.skillId,
            Name: skill.name
        }));
    }

    static normalizeWeeklyHours(weeklyHours) {
        const hoursArray = weeklyHours?.$values || weeklyHours || [];
        return hoursArray.map(hour => ({
            WeekStartDate: hour.weekStartDate,
            Hours: hour.hours
        }));
    }

    static buildSavePayload(resource) {
        const isUpdate = !!resource.Id;

        const basePayload = {
            name: resource.Name,
            designationId: resource.DesignationId || 1,
            billableId: resource.BillableId,
            availability: resource.Availability,
            weeklyHours: resource.WeeklyHours.map(wh => ({
                weekStartDate: wh.WeekStartDate,
                hours: wh.Hours
            }))
        };

        if (isUpdate) {
            basePayload.resourceId = resource.Id;
            basePayload.skillIds = resource.Skills.map(skill => skill.SkillId);
        } else {
            basePayload.resourceSkills = resource.Skills.map(skill => ({
                skillId: skill.SkillId
            }));
        }

        return basePayload;
    }
}