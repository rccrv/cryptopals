#include <cstdint>
#include <iostream>
#include <sstream>
#include <string>

using namespace std;

int main() {
	string s1 = "1c0111001f010100061a024b53535009181c";
	string s2 = "686974207468652062756c6c277320657965";
	stringstream r;
	uint8_t n1, n2;

	for (int i = 0; i < s1.length(); i += 2) {
		n1 = stoi("0x" + s1.substr(i, 2), NULL, 16);
		n2 = stoi("0x" + s2.substr(i, 2), NULL, 16);
		r << std::hex << (n1 ^ n2);
	}
	cout << r.str() << '\n';

	return 0;
}
