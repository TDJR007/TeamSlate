// ============================
// DATE UTILITIES MODULE
// Handles all date-related operations
// ============================

class DateUtils {
    static formatDate(dateInput) {
        const date = new Date(dateInput);
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        return `${day}/${month}`;
    }

    static createWeekColumns(startDate, endDate) {
        const mondays = [];
        const start = new Date(startDate);
        const end = new Date(endDate);

        // Adjust start to the previous Monday
        const day = start.getDay();
        const diffToMonday = (day + 6) % 7;
        start.setDate(start.getDate() - diffToMonday);

        let current = new Date(start);

        while (current <= end) {
            mondays.push(new Date(current));
            current.setDate(current.getDate() + 7);
        }

        return mondays;
    }

    static validateDateRange(startDate, endDate) {
        if (!startDate || !endDate) {
            alert("Please select both start and end date.");
            return false;
        }

        if (startDate > endDate) {
            return {
                valid: false,
                message: "End Date cannot be before start date."
            };
        }

        return { valid: true };
    }
}
