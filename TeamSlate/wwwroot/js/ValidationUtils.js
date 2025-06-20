// ============================
// VALIDATION UTILITIES MODULE
// Handles input validation
// ============================

class ValidationUtils {
    static validateHours(inputValue, inputElement) {
        let hours = inputValue || 0;

        if (hours < 0) {
            hours = 0;
            inputElement.value = 0;
            NotificationService.showToast("Negative hours not allowed! Set to 0.", "warning");
        } else if (hours > 40) {
            hours = 40;
            inputElement.value = 40;
            NotificationService.showToast("Maximum 40 hours per week allowed! Set to 40.", "warning");
        }

        return hours;
    }

    static validateHoursRealTime(inputElement) {
        const value = parseFloat(inputElement.value);
        if (value > 40) {
            inputElement.style.borderColor = '#ff4444';
            inputElement.title = 'Maximum 40 hours allowed per week';
        } else if (value < 0) {
            inputElement.style.borderColor = '#ff4444';
            inputElement.title = 'Negative hours not allowed';
        } else {
            inputElement.style.borderColor = '';
            inputElement.title = '';
        }
    }
}