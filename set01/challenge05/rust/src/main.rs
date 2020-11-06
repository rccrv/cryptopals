fn main() {
    let s = "Burning 'em, if you ain't quick and nimble\nI go crazy when I hear a cymbal";
    let v = vec!['I', 'C', 'E'];
    let mut iter = v.iter().cycle();

    let bytes = s.as_bytes();

    for i in bytes {
        let c = match iter.next() {
            Some(i) => *i,
            None => panic!("Should always receive a character from a cycled iterator")
        };
        print!("{:02x}", i ^ c as u8);
    }
    println!("");
}
