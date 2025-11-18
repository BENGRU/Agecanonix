import 'package:flutter/material.dart';
import 'dart:convert';
import 'package:http/http.dart' as http;

void main() {
  runApp(const MainApp());
}

class MainApp extends StatelessWidget {
  const MainApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Agecanonix App',
      theme: ThemeData(
        primarySwatch: Colors.blue,
        useMaterial3: true,
      ),
      home: const WeatherTestPage(),
    );
  }
}

class WeatherTestPage extends StatefulWidget {
  const WeatherTestPage({super.key});

  @override
  State<WeatherTestPage> createState() => _WeatherTestPageState();
}

class _WeatherTestPageState extends State<WeatherTestPage> {
  List<Map<String, dynamic>> _weatherData = [];
  bool _isLoading = false;
  String? _errorMessage;

  Future<void> _fetchWeatherData() async {
    setState(() {
      _isLoading = true;
      _errorMessage = null;
      // Ne pas effacer _weatherData pour éviter le clignotement
    });

    try {
      // Détection automatique de l'environnement (Codespaces vs local)
      final uri = Uri.base;
      final isCodespaces = uri.host.contains('github.dev');
      
      final apiUrl = isCodespaces
          ? uri.toString().replaceAll('-8080.', '-5000.').replaceAll(RegExp(r'/#?$'), '') + '/weatherforecast'
          : 'http://localhost:5000/weatherforecast';
      
      print('API URL: $apiUrl'); // Pour debug
      
      final response = await http.get(Uri.parse(apiUrl));

      if (response.statusCode == 200) {
        final List<dynamic> data = json.decode(response.body);
        setState(() {
          _weatherData = data.cast<Map<String, dynamic>>();
          _isLoading = false;
        });
      } else {
        setState(() {
          _errorMessage = 'Erreur: ${response.statusCode}';
          _isLoading = false;
        });
      }
    } catch (e) {
      setState(() {
        _errorMessage = 'Erreur de connexion: $e';
        _isLoading = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Test API Agecanonix'),
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
      ),
      body: SingleChildScrollView(
        child: Center(
          child: Padding(
            padding: const EdgeInsets.all(16.0),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                const Text(
                  'Test de l\'API WeatherForecast',
                  style: TextStyle(
                    fontSize: 24,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                const SizedBox(height: 32),
                ElevatedButton.icon(
                  onPressed: _isLoading ? null : _fetchWeatherData,
                  icon: const Icon(Icons.cloud),
                  label: const Text('Charger les températures'),
                  style: ElevatedButton.styleFrom(
                    padding: const EdgeInsets.symmetric(
                      horizontal: 32,
                      vertical: 16,
                    ),
                  ),
                ),
                const SizedBox(height: 32),
                if (_isLoading)
                  const CircularProgressIndicator()
                else if (_errorMessage != null)
                  Text(
                    _errorMessage!,
                    style: const TextStyle(
                      color: Colors.red,
                      fontSize: 16,
                    ),
                  )
                else if (_weatherData.isNotEmpty)
                  AnimatedSwitcher(
                    duration: const Duration(milliseconds: 300),
                    child: ListView.builder(
                      key: ValueKey(_weatherData.length),
                      shrinkWrap: true,
                      physics: const NeverScrollableScrollPhysics(),
                      itemCount: _weatherData.length,
                      itemBuilder: (context, index) {
                        final weather = _weatherData[index];
                        return Card(
                          key: ValueKey('${weather['date']}_${weather['temperatureC']}'),
                          margin: const EdgeInsets.symmetric(vertical: 8),
                        child: ListTile(
                          contentPadding: const EdgeInsets.symmetric(
                            horizontal: 16,
                            vertical: 12,
                          ),
                          leading: Icon(
                            Icons.thermostat,
                            color: _getTemperatureColor(
                              weather['temperatureC'] ?? 0,
                            ),
                            size: 32,
                          ),
                          title: Text(
                            '${weather['summary'] ?? 'N/A'}',
                            style: const TextStyle(
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                          subtitle: Text(
                            'Date: ${weather['date'] ?? 'N/A'}',
                          ),
                          trailing: Row(
                            mainAxisSize: MainAxisSize.min,
                            children: [
                              Column(
                                mainAxisAlignment: MainAxisAlignment.center,
                                crossAxisAlignment: CrossAxisAlignment.end,
                                mainAxisSize: MainAxisSize.min,
                                children: [
                                  Text(
                                    '${weather['temperatureC'] ?? 0}°C',
                                    style: const TextStyle(
                                      fontSize: 16,
                                      fontWeight: FontWeight.bold,
                                    ),
                                  ),
                                  const SizedBox(height: 2),
                                  Text(
                                    '${weather['temperatureF'] ?? 0}°F',
                                    style: TextStyle(
                                      fontSize: 12,
                                      color: Colors.grey[600],
                                    ),
                                  ),
                                ],
                              ),
                            ],
                          ),
                        ),
                      );
                    },
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Color _getTemperatureColor(int tempC) {
    if (tempC < 0) return Colors.blue;
    if (tempC < 10) return Colors.lightBlue;
    if (tempC < 20) return Colors.green;
    if (tempC < 30) return Colors.orange;
    return Colors.red;
  }
}
