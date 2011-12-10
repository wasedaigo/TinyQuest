//
//  ViewController.m
//  TinyQuest
//
//  Created by sato.daigo on 11/11/26.
//  Copyright (c) 2011年 __MyCompanyName__. All rights reserved.
//

#import "AdventureViewController.h"
#import "ItemDialogController.h"
#import "InventoryViewController.h"

@implementation AdventureViewController
@synthesize itemDialogController, inventoryViewController;
@synthesize controlPanelView, inventoryPanelView;

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Release any cached data, images, etc that aren't in use.
}

- (void)viewDidLoad
{
    [super viewDidLoad];

    // Initialize InventoryViewController
    self.inventoryViewController = [[InventoryViewController alloc] init];
    [self.inventoryPanelView addSubview:self.inventoryViewController.scrollView];
    [self.inventoryViewController setupInventory];
}

- (IBAction)gotoProfile
{
    if (!self.itemDialogController)
    {
        self.itemDialogController = [[ItemDialogController alloc] init];
        self.itemDialogController.delegate = self;
        [self.view addSubview:self.itemDialogController.view];
    }
    
    [self.itemDialogController slideIn];
    [self slideOut];
}

- (void)viewDidUnload
{
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (void)viewWillAppear:(BOOL)animated
{
    [super viewWillAppear:animated];
}

- (void)viewDidAppear:(BOOL)animated
{
    [super viewDidAppear:animated];
}

- (void)viewWillDisappear:(BOOL)animated
{
	[super viewWillDisappear:animated];
}

- (void)viewDidDisappear:(BOOL)animated
{
	[super viewDidDisappear:animated];
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        return (interfaceOrientation != UIInterfaceOrientationPortraitUpsideDown);
    } else {
        return YES;
    }
}

- (void)slideIn
{
    self.controlPanelView.userInteractionEnabled = NO;
    [UIView beginAnimations:nil context:nil];
    [UIView setAnimationDelegate:self];
    [UIView setAnimationDidStopSelector:@selector(onSlideFinished:finished:context:)];
    [UIView setAnimationDuration:0.5];
    [UIView setAnimationCurve:UIViewAnimationCurveEaseOut];
    self.controlPanelView.frame = CGRectMake(0, self.controlPanelView.frame.origin.y, self.controlPanelView.frame.size.width, self.controlPanelView.frame.size.height);
    [UIView commitAnimations];
}

- (void)slideOut
{
    self.controlPanelView.userInteractionEnabled = NO;
    [UIView beginAnimations:nil context:nil];
    [UIView setAnimationDelegate:self];
    [UIView setAnimationDidStopSelector:@selector(onSlideFinished:finished:context:)];
    [UIView setAnimationDuration:0.5];
    [UIView setAnimationCurve:UIViewAnimationCurveEaseOut];
    self.controlPanelView.frame = CGRectMake(-320, self.controlPanelView.frame.origin.y, self.controlPanelView.frame.size.width, self.controlPanelView.frame.size.height);
    [UIView commitAnimations];
}

-(void)onSlideFinished:(NSString *)animationID finished:(NSNumber *)finished context:(void *)context
{
    self.controlPanelView.userInteractionEnabled = YES;
}

- (void)glkViewControllerUpdate:(GLKViewController*)controller
{
}

- (void)glkView:(GLKView*)view drawInRect:(CGRect)rect
{
    
}


@end
