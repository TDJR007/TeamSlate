// ============================
// TABLE RENDERER MODULE
// Handles all UI rendering and DOM manipulation
// ============================

class TableRenderer {
    constructor(tableElement, state, eventHandlers) {
        this.table = tableElement;
        this.state = state;
        this.handlers = eventHandlers;
    }

    render(resourceData, weekColumns) {
        this.clear();
        this.renderHeader(weekColumns);
        this.renderBody(resourceData, weekColumns);
    }

    clear() {
        this.table.innerHTML = "";
    }

    renderHeader(weekColumns) {
        const headerRow = this.table.insertRow();
        const headers = [
            "Resource Name", "Designation", "Billable", "Skills", "Availability",
            ...weekColumns.map(week => DateUtils.formatDate(week)),
            "Update", "Delete"
        ];

        headers.forEach(headerText => {
            const th = document.createElement("th");
            th.textContent = headerText;
            headerRow.appendChild(th);
        });
    }

    renderBody(resourceData, weekColumns) {
        resourceData.forEach((resource, index) => {
            const row = this.table.insertRow();
            this.renderResourceRow(row, resource, index, weekColumns);
        });
    }

    renderResourceRow(row, resource, index, weekColumns) {
        // Name input
        this.appendInputCell(row, resource.Name, (value) => {
            resource.Name = value;
        });

        // Designation dropdown
        this.appendDesignationCell(row, resource);

        // Billable dropdown
        this.appendBillableCell(row, resource);

        // Skills multi-select
        this.appendSkillsCell(row, resource);

        // Availability input
        this.appendInputCell(row, resource.Availability, (value) => {
            resource.Availability = value;
        });

        // Weekly hours columns
        weekColumns.forEach(weekDate => {
            this.appendWeeklyHourCell(row, resource, weekDate);
        });

        // Action buttons
        this.appendActionButtons(row, resource, index, weekColumns);
    }

    appendInputCell(row, value, onChangeCallback) {
        const cell = row.insertCell();
        const input = document.createElement("input");
        input.value = value || "";
        input.onchange = (e) => onChangeCallback(e.target.value);
        cell.appendChild(input);
    }

    appendDesignationCell(row, resource) {
        const cell = row.insertCell();
        const select = document.createElement("select");

        const designations = this.state.designationMap.$values || this.state.designationMap;
        designations.forEach(designation => {
            const option = document.createElement("option");
            option.value = designation.id;
            option.text = designation.name;
            option.selected = designation.id === resource.DesignationId;
            select.appendChild(option);
        });

        select.onchange = (e) => {
            resource.DesignationId = parseInt(e.target.value);
        };

        cell.appendChild(select);
    }

    appendBillableCell(row, resource) {
        const cell = row.insertCell();
        const select = document.createElement("select");

        Object.keys(this.state.billableMap).forEach(key => {
            const option = document.createElement("option");
            option.value = key;
            option.text = this.state.billableMap[key];
            option.selected = parseInt(key) === resource.BillableId;
            select.appendChild(option);
        });

        select.onchange = (e) => {
            resource.BillableId = parseInt(e.target.value);
        };

        cell.appendChild(select);
    }

    appendSkillsCell(row, resource) {
        const cell = row.insertCell();
        const select = document.createElement("select");
        select.multiple = true;
        select.className = "skills-select";
        select.id = `skills-${Math.random().toString(36).substr(2, 9)}`;

        this.state.skillMap.forEach(skill => {
            const option = document.createElement("option");
            option.value = skill.id;
            option.text = skill.name;
            option.selected = resource.Skills.some(rs => rs.SkillId === skill.id);
            select.appendChild(option);
        });

        cell.appendChild(select);

        // Initialize Select2
        setTimeout(() => {
            $(select).select2({
                placeholder: "Select skills...",
                allowClear: true,
                width: '250px',
                dropdownParent: $('body'),
                closeOnSelect: false
            }).on('change', function () {
                const selectedOptions = $(this).find('option:selected');
                resource.Skills = Array.from(selectedOptions).map(option => ({
                    SkillId: parseInt(option.value),
                    Name: option.text
                }));
            });
        }, 0);
    }

    appendWeeklyHourCell(row, resource, weekDate) {
        const cell = row.insertCell();
        const input = document.createElement("input");
        input.type = "number";
        input.min = 0;
        input.max = 40;
        input.step = 0.5;

        const existingHour = resource.WeeklyHours.find(wh =>
            new Date(wh.WeekStartDate).toDateString() === new Date(weekDate).toDateString()
        );

        input.value = existingHour ? existingHour.Hours : 0;

        input.onchange = (e) => {
            const validatedHours = ValidationUtils.validateHours(parseFloat(e.target.value), input);

            if (existingHour) {
                existingHour.Hours = validatedHours;
            } else {
                resource.WeeklyHours.push({
                    WeekStartDate: weekDate,
                    Hours: validatedHours
                });
            }
        };

        input.oninput = (e) => ValidationUtils.validateHoursRealTime(e.target);

        cell.appendChild(input);
    }

    appendActionButtons(row, resource, index, weekColumns) {
        // Update button
        const updateCell = row.insertCell();
        const updateBtn = document.createElement("button");
        updateBtn.textContent = "💾";
        updateBtn.className = "btn-save";
        updateBtn.onclick = () => this.handlers.onSave(resource);
        updateCell.appendChild(updateBtn);

        // Delete button
        const deleteCell = row.insertCell();
        const deleteBtn = document.createElement("button");
        deleteBtn.textContent = "🗑️";
        deleteBtn.className = "btn-delete";
        deleteBtn.onclick = () => this.handlers.onDelete(resource, index, weekColumns);
        deleteCell.appendChild(deleteBtn);
    }
}