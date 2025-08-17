// Factory - Component factory utility
(function() {
    'use strict';
    
    window.Factory = {
        // Create component instance
        create: function(type, options) {
            switch (type) {
                case 'modal':
                    return this.createModal(options);
                case 'dropdown':
                    return this.createDropdown(options);
                case 'tooltip':
                    return this.createTooltip(options);
                default:
                    console.warn('Unknown component type:', type);
                    return null;
            }
        },
        
        // Create modal component
        createModal: function(options) {
            var modal = {
                element: null,
                options: options || {},
                
                show: function() {
                    if (this.element) {
                        this.element.style.display = 'block';
                        document.body.classList.add('modal-open');
                    }
                },
                
                hide: function() {
                    if (this.element) {
                        this.element.style.display = 'none';
                        document.body.classList.remove('modal-open');
                    }
                }
            };
            
            return modal;
        },
        
        // Create dropdown component
        createDropdown: function(options) {
            var dropdown = {
                element: null,
                options: options || {},
                
                toggle: function() {
                    if (this.element) {
                        this.element.classList.toggle('show');
                    }
                },
                
                hide: function() {
                    if (this.element) {
                        this.element.classList.remove('show');
                    }
                }
            };
            
            return dropdown;
        },
        
        // Create tooltip component
        createTooltip: function(options) {
            var tooltip = {
                element: null,
                options: options || {},
                
                show: function() {
                    if (this.element) {
                        this.element.style.display = 'block';
                    }
                },
                
                hide: function() {
                    if (this.element) {
                        this.element.style.display = 'none';
                    }
                }
            };
            
            return tooltip;
        }
    };
})();
