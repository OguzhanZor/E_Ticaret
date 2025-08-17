// Demo Chart.js - Sample charts for admin dashboard
(function() {
    'use strict';
    
    // Sample chart data and configurations
    window.DemoCharts = {
        // Sample line chart
        lineChart: function(canvasId, data) {
            var ctx = document.getElementById(canvasId);
            if (!ctx) return;
            
            new Chart(ctx, {
                type: 'line',
                data: data || {
                    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
                    datasets: [{
                        label: 'Sales',
                        data: [12, 19, 3, 5, 2, 3],
                        borderColor: 'rgb(75, 192, 192)',
                        tension: 0.1
                    }]
                },
                options: {
                    responsive: true,
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        },
        
        // Sample bar chart
        barChart: function(canvasId, data) {
            var ctx = document.getElementById(canvasId);
            if (!ctx) return;
            
            new Chart(ctx, {
                type: 'bar',
                data: data || {
                    labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
                    datasets: [{
                        label: 'Votes',
                        data: [12, 19, 3, 5, 2, 3],
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(255, 205, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)',
                            'rgba(153, 102, 255, 0.2)',
                            'rgba(255, 159, 64, 0.2)'
                        ],
                        borderColor: [
                            'rgba(255, 99, 132, 1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 205, 86, 1)',
                            'rgba(75, 192, 192, 1)',
                            'rgba(153, 102, 255, 1)',
                            'rgba(255, 159, 64, 1)'
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        },
        
        // Sample pie chart
        pieChart: function(canvasId, data) {
            var ctx = document.getElementById(canvasId);
            if (!ctx) return;
            
            new Chart(ctx, {
                type: 'pie',
                data: data || {
                    labels: ['Red', 'Blue', 'Yellow'],
                    datasets: [{
                        data: [300, 50, 100],
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.8)',
                            'rgba(54, 162, 235, 0.8)',
                            'rgba(255, 205, 86, 0.8)'
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true
                }
            });
        }
    };
    
    // Initialize charts when DOM is ready
    $(document).ready(function() {
        // Sample chart initializations can go here
        console.log('Demo charts loaded');
    });
    
})();
