// ============================
// API SERVICE MODULE
// Handles all HTTP requests and data fetching
// ============================

class ApiService {
    constructor() {
        this.baseUrl = '/api';
    }

    async fetchDesignations() {
        try {
            const response = await fetch(`${this.baseUrl}/designation`);
            if (!response.ok) throw new Error(`HTTP ${response.status}`);
            return await response.json();
        } catch (error) {
            console.error("Failed to fetch designations:", error);
            throw new Error("Designation fetch failed");
        }
    }

    async fetchSkills() {
        try {
            const response = await fetch(`${this.baseUrl}/skill`);
            if (!response.ok) throw new Error(`HTTP ${response.status}`);
            const data = await response.json();
            return data.$values || data;
        } catch (error) {
            console.error("Failed to fetch skills:", error);
            throw new Error("Skill fetch failed");
        }
    }

    async fetchResourcesWithHours(startDate, endDate) {
        try {
            const [designationRes, skillRes, resourceRes] = await Promise.all([
                fetch(`${this.baseUrl}/designation`),
                fetch(`${this.baseUrl}/skill`),
                fetch(`${this.baseUrl}/resource/with-hours?start=${startDate}&end=${endDate}`)
            ]);

            if (!designationRes.ok || !skillRes.ok || !resourceRes.ok) {
                throw new Error("One or more API calls failed");
            }

            return {
                designations: await designationRes.json(),
                skills: await skillRes.json(),
                resources: await resourceRes.json()
            };
        } catch (error) {
            console.error("Failed to fetch resources:", error);
            throw new Error("Resource fetch failed");
        }
    }

    async saveResource(payload, isUpdate = false) {
        const url = isUpdate
            ? `${this.baseUrl}/resource/update-resource-with-hours`
            : `${this.baseUrl}/resource/add-resource`;
        const method = isUpdate ? 'PUT' : 'POST';

        return fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });
    }

    async deleteResource(resourceId) {
        return fetch(`${this.baseUrl}/resource/${resourceId}`, {
            method: 'DELETE'
        });
    }
}
