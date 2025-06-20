// ============================
// NOTIFICATION SERVICE MODULE
// Handles user notifications and feedback
// ============================

class NotificationService {
    static showToast(message, type) {
        if (typeof showToast === 'function') {
            showToast(message, type);
        } else {
            console.log(`${type.toUpperCase()}: ${message}`);
        }
    }
}