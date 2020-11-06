#include <cstdint>
#include <iostream>
#include <string>

using namespace std;

int main() {
	string s = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";
	string r = "";
	const char d[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
	uint8_t n1, n2, n3;
	int d1, d2, d3, d4;

	for (int i = 0; i < s.length(); i += 6) {
		n1 = stoi("0x" + s.substr(i, 2), NULL, 16);
		n2 = stoi("0x" + s.substr(i + 2, 2), NULL, 16);
		n3 = stoi("0x" + s.substr(i + 4, 2), NULL, 16);
		d1 = n1 & 0b11111100 >> 2;
		d2 = (n1 & 0b00000011) << 4 | (n2 & 0b11110000) >> 4;
		d3 = (n2 & 0b00001111) << 2 | (n3 & 0b11000000) >> 6;
		d4 = n3 & 0b00111111;
		r += d[d1];
		r += d[d2];
		r += d[d3];
		r += d[d4];
	}
	cout << r << '\n';

	return 0;
}
