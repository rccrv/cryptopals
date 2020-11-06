fn main() {
    let s1 = "1c0111001f010100061a024b53535009181c";
    let s2 = "686974207468652062756c6c277320657965";
    let mut r = String::new();
    let (mut n1, mut n2): (u8, u8);

    for i in (0..s1.len()).step_by(2) {
        n1 = u8::from_str_radix( &s1[i..i+2], 16).unwrap_or(0);
        n2 = u8::from_str_radix( &s2[i..i+2], 16).unwrap_or(0);
        r += format!("{:x}", n1 ^ n2).as_mut_str();
    }
    println!("{}", r);
}
