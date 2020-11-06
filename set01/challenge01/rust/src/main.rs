fn main() {
    let s = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";
    let mut r = String::new();
    let d = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".as_bytes();
    let (mut n1, mut n2, mut n3): (u8, u8, u8);
    let (mut d1, mut d2, mut d3, mut d4): (usize, usize, usize, usize);

    for i in (0..s.len()).step_by(6) {
        n1 = u8::from_str_radix( &s[i..i+2], 16).unwrap_or(0);
        n2 = u8::from_str_radix( &s[i+2..i+4], 16).unwrap_or(0);
        n3 = u8::from_str_radix( &s[i+4..i+6], 16).unwrap_or(0);
        d1 = ((n1 & 0b11_11_11_00) >> 2) as usize;
        d2 = ((n1 & 0b00_00_00_11) << 4 | (n2 & 0b11_11_00_00) >> 4) as usize;
        d3 = ((n2 & 0b00_00_11_11) << 2 | (n3 & 0b11_00_00_00) >> 6) as usize;
        d4 = (n3 & 0b00_11_11_11) as usize;
        r.push(d[d1] as char);
        r.push(d[d2] as char);
        r.push(d[d3] as char);
        r.push(d[d4] as char);

    }
    println!("{}", r);
}
