// ============================
// MAIN RESOURCE GRID CONTROLLER
// Orchestrates all modules and handles business logic
// ============================

class ResourceGrid {
    constructor() {
        this.state = {
            startDate: null,
            endDate: null,
            resourceData: [],
            designationMap: [],
            skillMap: [],
            billableMap: { 1: 'Yes', 2: 'No' }
        };

        this.elements = {
            table: document.getElementById("resourceTable"),
            fetchBtn: document.getElementById("fetchBtn"),
            addBtn: document.getElementById("addResourceBtn"),
            startDateInput: document.getElementById("startDate"),
            endDateInput: document.getElementById("endDate")
        };

        // Initialize services
        this.apiService = new ApiService();
        this.renderer = new TableRenderer(this.elements.table, this.state, {
            onSave: (resource) => this.handleSaveResource(resource),
            onDelete: (resource, index, weekColumns) => this.handleDeleteResource(resource, index, weekColumns)
        });

        this.init();
    }

    init() {
        this.bindEvents();
        this.loadInitialData();
    }

    bindEvents() {
        this.elements.fetchBtn.addEventListener("click", () => this.handleFetchResources());
        this.elements.addBtn.addEventListener("click", () => this.handleAddResource());
    }

    async loadInitialData() {
        try {
            const [designations, skills] = await Promise.all([
                this.apiService.fetchDesignations(),
                this.apiService.fetchSkills()
            ]);

            this.state.designationMap = designations;
            this.state.skillMap = skills;
        } catch (error) {
            console.error("Failed to load initial data:", error);
            NotificationService.showToast("Failed to load initial data", "error");
        }
    }

    async handleFetchResources() {
        const startDate = this.elements.startDateInput.value;
        const endDate = this.elements.endDateInput.value;

        const validation = DateUtils.validateDateRange(startDate, endDate);
        if (!validation.valid) {
            NotificationService.showToast(validation.message, "error");
            return;
        }

        this.state.startDate = startDate;
        this.state.endDate = endDate;

        try {
            const data = await this.apiService.fetchResourcesWithHours(startDate, endDate);

            // Update state
            this.state.designationMap = data.designations;
            this.state.skillMap = data.skills.$values || data.skills;
            this.state.resourceData = DataTransformer.normalizeResourceData(data.resources);

            // Render table
            const weekColumns = DateUtils.createWeekColumns(startDate, endDate);
            this.renderer.render(this.state.resourceData, weekColumns);

            NotificationService.showToast("Resource data loaded successfully", "success");
        } catch (error) {
            NotificationService.showToast("Failed to fetch resource data", "error");
        }
    }

    handleAddResource() {
        const validation = DateUtils.validateDateRange(this.state.startDate, this.state.endDate);
        if (!validation.valid) {
            alert("Please select start and end date first.");
            return;
        }

        const weekColumns = DateUtils.createWeekColumns(this.state.startDate, this.state.endDate);
        const newResource = ResourceFactory.createEmptyResource(weekColumns, this.state.designationMap);

        this.state.resourceData.push(newResource);
        this.renderer.render(this.state.resourceData, weekColumns);
    }

    async handleSaveResource(resource) {
        const payload = DataTransformer.buildSavePayload(resource);
        const isUpdate = !!resource.Id;

        console.log('Sending payload:', JSON.stringify(payload, null, 2));

        try {
            const response = await this.apiService.saveResource(payload, isUpdate);

            if (response.ok) {
                const message = isUpdate ? "Resource updated successfully!" : "Resource added successfully!";
                NotificationService.showToast(message, "success");
                this.handleFetchResources(); // Refresh data
            } else {
                const errorData = await response.json();
                console.error('Save error response:', errorData);

                let errorMessage = "Failed to save resource";
                if (errorData.errors) {
                    const validationErrors = Object.values(errorData.errors).flat();
                    errorMessage += ": " + validationErrors.join(", ");
                }

                NotificationService.showToast(errorMessage, "error");
            }
        } catch (error) {
            console.error("Save error:", error);
            NotificationService.showToast("Unexpected error occurred", "error");
        }
    }

    async handleDeleteResource(resource, index, weekColumns) {
        // If new resource (no ID), just remove from array
        if (!resource.Id) {
            this.state.resourceData.splice(index, 1);
            this.renderer.render(this.state.resourceData, weekColumns);
            return;
        }

        if (!confirm("Are you sure you want to delete this resource permanently?")) {
            return;
        }

        try {
            const response = await this.apiService.deleteResource(resource.Id);

            if (response.status === 204) {
                this.state.resourceData.splice(index, 1);
                this.renderer.render(this.state.resourceData, weekColumns);
                NotificationService.showToast("Resource deleted successfully", "success");
            } else {
                NotificationService.showToast("Failed to delete resource", "error");
            }
        } catch (error) {
            console.error("Delete error:", error);
            NotificationService.showToast("Error occurred while deleting", "error");
        }
    }
}

// ============================
// INITIALIZATION
// ============================

document.addEventListener('DOMContentLoaded', () => {
    const requiredElements = [
        'resourceTable', 'fetchBtn', 'addResourceBtn', 'startDate', 'endDate'
    ];

    const missingElements = requiredElements.filter(id => !document.getElementById(id));

    if (missingElements.length > 0) {
        console.error('Missing required DOM elements:', missingElements);
        return;
    }

    window.resourceGrid = new ResourceGrid();
});

// Export for module systems
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        ResourceGrid,
        ApiService,
        DataTransformer,
        DateUtils,
        TableRenderer,
        ValidationUtils,
        NotificationService,
        ResourceFactory
    };
}
